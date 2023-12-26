import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import {MatTab, MatTabChangeEvent, MatTabGroup, MatTabsModule} from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[MemberMessagesComponent, CommonModule, MatTabsModule, NgxGalleryModule]
})
export class MemberDetailComponent {
  @ViewChild('memberTabs') memberTabs?: MatTab; 
  member: Member | undefined;
  galleryOptions: NgxGalleryOptions[] = []; 
  galleryImages: NgxGalleryImage[] = []; 
  activeTab?: MatTab | undefined; 
  messages: Message[] = [];


  constructor(private memberService : MembersService, private route: ActivatedRoute, private messageService: MessageService) {} 

  ngOnInit(): void {
    this.loadMember();

    // options for ngx photos 
    this.galleryOptions = [
      {
        width: "800px",
        height: "500px",
        imagePercent: 100,
        thumbnailsColumns: 4, 
        imageAnimation: NgxGalleryAnimation.Fade,
        preview: false
      }
    ]
  }

  getImages(){
    if(!this.member) return []; 
    const imageUrls = []; 
    for (const photo of this.member.photos) 
    {
      imageUrls.push({
        small: photo.url, 
        medium: photo.url,
        big: photo.url
      })
    }
    return imageUrls;
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get("username");
    if(!username) return
    this.memberService.getMember(username).subscribe({
      next: member => {this.member = member; 
            // get photos of members
    this.galleryImages = this.getImages(); 
      }
    })
  }

  loadMessages() {
    if (this.member?.userName) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  onTabActivated(data: MatTabChangeEvent) {
    this.activeTab = data.tab; 
    if(this.activeTab.textLabel ==='Messages') {this.loadMessages()}; 
  }

  onTabChange(event: MatTabChangeEvent): void {
    const selectedTabIndex = event.index;
    const selectedTabLabel = event.tab.textLabel;

    console.log(`Selected Tab Index: ${selectedTabIndex}`);
    console.log(`Selected Tab Label: ${selectedTabLabel}`);
  }
}

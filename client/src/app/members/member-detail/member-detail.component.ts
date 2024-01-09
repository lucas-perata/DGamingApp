import { ChangeDetectorRef, Component, OnDestroy, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute, Params } from '@angular/router';
import { NgxGalleryAnimation, NgxGalleryImage, NgxGalleryModule, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import {MatTab, MatTabChangeEvent, MatTabGroup, MatTabsModule} from '@angular/material/tabs';
import { CommonModule } from '@angular/common';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { HasRoleDirective } from 'src/app/_directives/has-role.directive';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs';


@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports:[MemberMessagesComponent, CommonModule, MatTabsModule, NgxGalleryModule]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild(MatTabGroup) memberTabs?: MatTabGroup;
  @ViewChildren(MatTab) matTabs?: QueryList<MatTab>;
  member: Member = {} as Member;
  galleryOptions: NgxGalleryOptions[] = []; 
  galleryImages: NgxGalleryImage[] = []; 
  activeTab?: MatTab | undefined; 
  messages: Message[] = [];
  changeTab?: MatTabChangeEvent;
  matTabGroup?: MatTabGroup;
  user?: User;

  constructor(private memberService: MembersService, private route: ActivatedRoute, private messageService: MessageService,
    public presenceService: PresenceService, private accountService: AccountService) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user;
        }
      })
    } 

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  ngAfterViewInit(): void {
    // Check if the tabGroup and tabs are initialized
    if (this.memberTabs && this.matTabs) {
      // Call navigateToTabByLabel here if needed
      this.route.queryParams.subscribe({
        next: params => {
          this.navigateToTabByLabel(params['tab']) 
          const tabLabel = params['tab'];}}
          )}
   }
  

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => this.member = data['member'] 
    }); 

    this.getImages(); 
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

  loadMessages() {
    if (this.member?.userName) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  onTabActivated(data: MatTabChangeEvent) {
    this.activeTab = data.tab; 
    if(this.activeTab.textLabel ==='Messages' && this.user) {this.messageService.createHubConnection(this.user, this.member.userName);}
    else {this.messageService.stopHubConnection()}} 
  

  // Method to navigate to a tab based on its label
  navigateToTabByLabel(label: string) {
    if (this.matTabs) {
      const tabToNavigate = this.matTabs.find((tab) => tab.textLabel === label);

      if (tabToNavigate) {
        const tabIndex = this.matTabs.toArray().indexOf(tabToNavigate);
        this.memberTabs!.selectedIndex = tabIndex;
      }
    }
  }
}

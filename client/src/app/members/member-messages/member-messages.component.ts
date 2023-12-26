import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';
import { TimeagoModule } from "ngx-timeago";

@Component({
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule, TimeagoModule]
})
export class MemberMessagesComponent implements OnInit {
  @Input() username?: string; 
  @Input() messages: Message[] = [];

  currentDate = Date.now();


  constructor(private messageService: MessageService){}
  
  ngOnInit(): void {
  }


}

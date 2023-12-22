import { Component } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent { 
  messages?: Message[];  
  pagination?: Pagination; 
  container = "Unread"; 
  pageNumber = 1; 
  pageSize = 5; 
  pageIndex = 0;

  constructor(private messageService: MessageService){}

  ngOnInit():void {
    this.loadMessages(); 
  }

  loadMessages() {
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response => {
        this.messages = response.result; 
        this.pagination = response.pagination
      }
    })
  }

  handlePageEvent(e: PageEvent) {
    if (this.messages)
    {
      this.pageSize = e.pageSize;
      this.pageIndex = 0;
      this.pageNumber = e.pageIndex + 1;
      this.loadMessages();
    }
  }}

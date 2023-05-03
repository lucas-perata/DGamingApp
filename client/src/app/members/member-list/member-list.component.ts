import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Observable } from 'rxjs/internal/Observable';
import { Member } from 'src/app/_models/member';
import { PaginatedResults, Pagination } from 'src/app/_models/pagination';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent {
  members: Member[] = []; 
  pageEvent: PageEvent | undefined;
  pagination: Pagination | undefined;
  pageNumber = 1; 
  pageSize = 5; 
  pageIndex = 0; 
  hidePageSize = false;
  showPageSizeOptions = true;
  showFirstLastButtons = true;
  disabled = false;


  constructor(private memberService: MembersService) {}

  ngOnInit(): void {
    this.loadMembers(); 
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.members = response.result; 
          this.pagination = response.pagination;
        }
      }
    })
  }
  
  handlePageEvent(e: PageEvent) {
    this.pageEvent = e;
    this.pageSize = e.pageSize;
    this.pageIndex = 1;
    this.pageNumber = e.pageIndex + 1;
    this.loadMembers();
  }


}


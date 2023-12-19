import { Component, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { take } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { Member } from 'src/app/_models/member';
import { PaginatedResults, Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
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
  userParams: UserParams | undefined; 
  pageIndex = 0; 
  hidePageSize = false;
  showPageSizeOptions = true;
  showFirstLastButtons = true;
  disabled = false;
  genderList = [{value: 'male', display: "Males"}, {value: 'female', display: "Females"}]; 
  orderByList = [{value: "lastActive", display: "Last active"}, {value: "created", display: "New members"}];


  constructor(private memberService: MembersService) {
    this.userParams = this.memberService.getUserParams();
  }

  ngOnInit(): void {
    this.loadMembers(); 
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: response => {
          if (response.result && response.pagination) {
            this.members = response.result; 
            this.pagination = response.pagination;
          }
        }
      })
    }  
  }

  resetFilters(){
      this.userParams = this.memberService.resetUserParams(); 
      this.loadMembers(); 
  }
  
  handlePageEvent(e: PageEvent) {
    if (this.userParams)
    {
      this.userParams.pageSize = e.pageSize;
      this.pageIndex = 0;
      this.userParams.pageNumber = e.pageIndex + 1;
      this.loadMembers();
    }
  }


}


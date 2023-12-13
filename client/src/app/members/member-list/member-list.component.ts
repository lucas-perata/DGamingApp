import { Component } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
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
  user: User | undefined; 
  pageIndex = 0; 
  hidePageSize = false;
  showPageSizeOptions = true;
  showFirstLastButtons = true;
  disabled = false;
  genderList = [{value: 'male', display: "Males"}, {value: 'female', display: "Females"}]; 
  orderByList = [{value: "lastActive", display: "Last active"}, {value: "created", display: "New members"}];


  constructor(private memberService: MembersService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user){
          this.userParams = new UserParams(user);
          this.user = user; 
        }
      }
    })
  }

  ngOnInit(): void {
    this.loadMembers(); 
  }

  loadMembers() {
    if (!this.userParams) return; 
    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.members = response.result; 
          this.pagination = response.pagination;
        }
      }
    })
  }

  resetFilters(){
    if (this.user){
      this.userParams = new UserParams(this.user); 
      this.loadMembers(); 
    }
  }
  
  handlePageEvent(e: PageEvent) {
    if (this.userParams && this.userParams?.pageNumber !== e.pageIndex)
    {
    this.pageEvent = e;
    this.userParams.pageSize = e.pageSize;
    this.pageIndex = 1;
    this.userParams.pageNumber = e.pageIndex + 1;
    this.loadMembers();
    }
  }


}


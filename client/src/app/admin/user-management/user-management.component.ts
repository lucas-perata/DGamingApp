import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import {MatDialog, MatDialogModule} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css'],
  standalone: true,
  imports: [MatButtonModule, MatDialogModule, CommonModule, FormsModule],
})

export class UserManagementComponent implements OnInit{
  users: User[] = []; 
  availableRoles = [
    'Admin',
    'Moderator',
    'Member'
  ]
  

  constructor(public dialog: MatDialog, private adminService: AdminService){

  }

  ngOnInit(): void {
    this.getUsersWithRoles(); 
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: users => this.users = users
    })
  }

    openDialog(user: User) {
      let dialogRef = this.dialog.open(RolesModalComponent, {
        data: {
          username: user.username,
          availableRoles: this.availableRoles,
          selectedRoles: [...user.roles],
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result && result.selectedRoles) {
          const selectedRoles = result.selectedRoles;
         
         if(!this.arrayEqual(selectedRoles!, user.roles)){
          this.adminService.updateUserRoles(user.username  , selectedRoles).subscribe(
            {next: roles => {user.roles = roles}},
            
           )
         }
        }
      });
    }

    private arrayEqual(arr1: any[], arr2: any[]) {
      return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort); 
    }
  
}

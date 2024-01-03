import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogModule, MatDialogRef} from '@angular/material/dialog';
import {MatButtonModule} from '@angular/material/button';
import { FormsModule, NgForm } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormField, MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import {MatCheckboxChange, MatCheckboxModule} from '@angular/material/checkbox';


@Component({
  selector: 'app-roles-modal',
  standalone: true,
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css'],
  imports: [MatButtonModule, MatDialogModule, FormsModule, CommonModule, MatFormFieldModule, MatInputModule, MatCheckboxModule],
})

export class RolesModalComponent implements OnInit {
  username = '';
  availableRoles: any[] = []; 
  selectedRoles: any[] = []; 

  constructor(private dialogRef: MatDialogRef<RolesModalComponent>, public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) 
  public data: {username: string, availableRoles: [], selectedRoles: []}) {}

  ngOnInit() {
  }
    

  openDialog() {
    const dialogRef = this.dialog.open(RolesModalComponent);

    dialogRef.afterClosed().subscribe(result => {
     
    });
  }

  save() {
      this.dialogRef.close({ selectedRoles: this.selectedRoles })
    
    ;
  }

  updateChecked(checkedValue: string, event: MatCheckboxChange){

    const checked = JSON.stringify(this.data.selectedRoles).includes(checkedValue);
    
    console.log(this.selectedRoles.indexOf(checkedValue)); 

    // Toggle the selected state for the role

    // If the role is deselected, remove it from the array
    if (checked == true) {
      const index = this.selectedRoles.indexOf(checkedValue);
      if (index !== -1) {
        this.selectedRoles.splice(index, 1);
      }
      console.log(this.selectedRoles, "true"); 
    } 
    if (checked == true) {
      const index = this.selectedRoles.indexOf(checkedValue);
      if (index !== -1) {
        this.selectedRoles.splice(index, 1);
      }
      console.log(this.selectedRoles, "true"); 
    } 
    if (!checked) {
      const index = this.selectedRoles.indexOf(checkedValue); 
      index !== -1 ? this.selectedRoles.splice(index, 1) : this.selectedRoles.push(checkedValue) && this.selectedRoles.push(this.data.selectedRoles);
      console.log("false", this.selectedRoles)
    }

  }
}

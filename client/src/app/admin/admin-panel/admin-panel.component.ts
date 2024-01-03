import { Component, NgModule, OnInit } from '@angular/core';
import { AccountService } from 'src/app/_services/account.service';


@Component({
  selector: 'app-admin-panel',
  templateUrl: './admin-panel.component.html',
  styleUrls: ['./admin-panel.component.css'],
})

export class AdminPanelComponent implements OnInit{
  
  constructor(public accountService: AccountService){}
  
  ngOnInit(): void {
  }

 
}

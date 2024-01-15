import {Injectable} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {ConfirmDialogComponent} from '../modals/confirm-dialog/confirm-dialog.component';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  constructor(public dialog: MatDialog) {}

  confirm(message: string): Observable<boolean> {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: {message: message},
    });

    return dialogRef.afterClosed();
  }
}
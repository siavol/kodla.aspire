import { Component, Input } from '@angular/core';
import { Meetup } from '../../model/meetup-types';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-request-attendee',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './request-attendee.component.html',
  styleUrl: './request-attendee.component.css'
})
export class RequestAttendeeComponent {
  @Input() meetup!: Meetup;

  userName: string = '';
  successMessage: string = '';

  constructor(private http: HttpClient) {}

  requestAttendee(): void {
    if (!this.meetup) return;

    const requestBody = {
      userName: this.userName
    };
    this.http
      .post(`api/meetups/${this.meetup.meetupId}/attendies`, requestBody)
      .subscribe({
        next: (data: any) => {
          this.successMessage = data.message;
          this.userName = '';
        },
        error: (err) => {
          console.error('Error requesting attendee:', err);
        }
      });
  }

}

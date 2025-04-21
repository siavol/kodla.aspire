import { Component, Input } from '@angular/core';
import { Meetup, RequestAttendeeResponse } from '../../model/meetup-types';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-request-attendee',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './request-attendee.component.html',
  styleUrl: './request-attendee.component.css'
})
export class RequestAttendeeComponent {
  @Input() meetup!: Meetup;

  userName: string = '';
  successMessage: string = '';
  requestId?: string;

  constructor(private http: HttpClient) {}

  requestAttendee(): void {
    if (!this.meetup) return;

    const requestBody = {
      userName: this.userName
    };
    this.http
      .post<RequestAttendeeResponse>(`api/meetups/${this.meetup.meetupId}/attendies`, requestBody)
      .subscribe({
        next: (data) => {
          this.successMessage = data.message;
          this.requestId = data.requestId;
          this.userName = '';
        },
        error: (err) => {
          console.error('Error requesting attendee:', err);
        }
      });
  }
}

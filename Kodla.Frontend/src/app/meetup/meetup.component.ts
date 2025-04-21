import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Meetup } from '../../model/meetup-types';
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-meetup',
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './meetup.component.html',
  styleUrl: './meetup.component.css'
})
export class MeetupComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient) {}

  meetup?: Meetup;
  userName: string = '';
  successMessage: string = '';

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const meetupId = params.get('meetupId');

      this.http.get<Meetup>(`api/meetups/${meetupId}`).subscribe({
        next: result => this.meetup = result,
        error: err => console.error('Error fetching meetup details:', err)
      });
    });
  }

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

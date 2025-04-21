import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Meetup } from '../../model/meetup-types';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-meetup',
  imports: [
    CommonModule
  ],
  templateUrl: './meetup.component.html',
  styleUrl: './meetup.component.css'
})
export class MeetupComponent implements OnInit {
  constructor(
    private route: ActivatedRoute,
    private http: HttpClient) {}

  // meetupId: string | null = null;
  meetup?: Meetup;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const meetupId = params.get('meetupId');

      this.http.get<Meetup>(`api/meetups/${meetupId}`).subscribe({
        next: result => this.meetup = result,
        error: err => console.error('Error fetching meetup details:', err)
      });
    });
  }
}

import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

type Meetup = {
  meetupId: number;
  name: string;
  description: string;
  date: string;
};

@Component({
  selector: 'app-root',
  imports: [
    CommonModule,
    RouterOutlet,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Kodla Meetups';
  meetups: Meetup[] = [];

  constructor(private http: HttpClient) {
  }
  ngOnInit(): void {
    this.http.get<Meetup[]>('api/meetups').subscribe({
      next: result => this.meetups = result,
      error: console.error
    });
  }
}

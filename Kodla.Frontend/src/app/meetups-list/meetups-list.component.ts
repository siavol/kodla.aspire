import { Component, OnInit } from '@angular/core';
import { Meetup } from '../../model/meetup-types';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-meetups-list',
  imports: [
    CommonModule
  ],
  templateUrl: './meetups-list.component.html',
  styleUrl: './meetups-list.component.css'
})
export class MeetupsListComponent implements OnInit {
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

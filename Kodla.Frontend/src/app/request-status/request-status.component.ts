import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AttendeeRequestStatus } from '../../model/meetup-types';

@Component({
  selector: 'app-request-status',
  imports: [
    CommonModule
  ],
  templateUrl: './request-status.component.html',
  styleUrl: './request-status.component.css'
})
export class RequestStatusComponent implements OnInit {
  requestStatus?: AttendeeRequestStatus;

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const requestId = params.get('requestId');

      this.http.get<AttendeeRequestStatus>(`api/requests/${requestId}`).subscribe({
        next: result => this.requestStatus = result,
        error: err => console.error('Error fetching request status:', err)
      });
    });
  }

}

import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-meetup',
  imports: [],
  templateUrl: './meetup.component.html',
  styleUrl: './meetup.component.css'
})
export class MeetupComponent {
  constructor(private route: ActivatedRoute) {}

  meetupId: string | null = null;

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.meetupId = params.get('meetupId');
    });
  }
}

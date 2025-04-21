import { Routes } from '@angular/router';
import { MeetupsListComponent } from './meetups-list/meetups-list.component';
import { MeetupComponent } from './meetup/meetup.component';

export const routes: Routes = [
  { path: '', component: MeetupsListComponent },
  { path: 'meetup/:meetupId', component: MeetupComponent },
];

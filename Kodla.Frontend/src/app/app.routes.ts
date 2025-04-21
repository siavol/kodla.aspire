import { Routes } from '@angular/router';
import { MeetupsListComponent } from './meetups-list/meetups-list.component';
import { MeetupComponent } from './meetup/meetup.component';
import { RequestStatusComponent } from './request-status/request-status.component';

export const routes: Routes = [
  { path: '', component: MeetupsListComponent },
  { path: 'meetup/:meetupId', component: MeetupComponent },
  { path: 'request-status/:requestId', component: RequestStatusComponent },
];

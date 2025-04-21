import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MeetupsListComponent } from './meetups-list/meetups-list.component';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    MeetupsListComponent
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Kodla Meetups';
}

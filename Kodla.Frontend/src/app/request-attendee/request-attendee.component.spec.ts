import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestAttendeeComponent } from './request-attendee.component';

describe('RequestAttendeeComponent', () => {
  let component: RequestAttendeeComponent;
  let fixture: ComponentFixture<RequestAttendeeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RequestAttendeeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RequestAttendeeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

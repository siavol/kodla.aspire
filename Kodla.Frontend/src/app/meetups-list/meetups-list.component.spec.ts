import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeetupsListComponent } from './meetups-list.component';

describe('MeetupsListComponent', () => {
  let component: MeetupsListComponent;
  let fixture: ComponentFixture<MeetupsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MeetupsListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MeetupsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViedoRoomComponent } from './viedo-room.component';

describe('ViedoRoomComponent', () => {
  let component: ViedoRoomComponent;
  let fixture: ComponentFixture<ViedoRoomComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ViedoRoomComponent]
    });
    fixture = TestBed.createComponent(ViedoRoomComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

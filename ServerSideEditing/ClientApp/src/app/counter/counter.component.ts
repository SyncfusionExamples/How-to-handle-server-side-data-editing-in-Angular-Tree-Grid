import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  standalone: true,
  imports: [CommonModule]
})
export class CounterComponent {
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}

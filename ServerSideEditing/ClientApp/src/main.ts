import { enableProdMode } from '@angular/core';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter, Routes } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { ToolbarService, EditService } from '@syncfusion/ej2-angular-treegrid';

import { AppComponent } from './app/app.component';
import { HomeComponent } from './app/home/home.component';
import { CounterComponent } from './app/counter/counter.component';
import { FetchDataComponent } from './app/fetch-data/fetch-data.component';
import { environment } from './environments/environment';

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}

const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
];

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
];

if (environment.production) {
  enableProdMode();
}

bootstrapApplication(AppComponent, {
  providers: [
    ...providers,
    provideHttpClient(),
    provideRouter(routes),
    ToolbarService,
    EditService
  ]
}).catch(err => console.log(err));

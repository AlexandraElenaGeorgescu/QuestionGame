import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthComponentComponent } from './components/auth-component/auth-component.component';
import { GameComponentComponent } from './components/game-component/game-component.component';
import { ScoreComponentComponent } from './components/score-component/score-component.component';
import { TimerComponentComponent } from './components/timer-component/timer-component.component';
import { EndGameComponentComponent } from './components/end-game-component/end-game-component.component';
import { LogoutComponentComponent } from './components/logout-component/logout-component.component';
import { ReportPlayerComponentComponent } from './components/report-player-component/report-player-component.component';
import { ErasePlayerComponentComponent } from './components/erase-player-component/erase-player-component.component';
import { FormsModule } from '@angular/forms';
import { AuthService } from './services/auth-service.service';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    AuthComponentComponent,
    GameComponentComponent,
    ScoreComponentComponent,
    TimerComponentComponent,
    EndGameComponentComponent,
    LogoutComponentComponent,
    ReportPlayerComponentComponent,
    ErasePlayerComponentComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }

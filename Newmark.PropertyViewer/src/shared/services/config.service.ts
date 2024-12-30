import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfigService {
  private config: any;

  constructor(private http: HttpClient) { }

  loadConfig(): Observable<any> {
    return this.http.get('/assets/config.json');
  }

  getConfig(key: string): any {
    return this.config ? this.config[key] : null;
  }

  setConfig(config: any): void {
    this.config = config;
  }
  
}

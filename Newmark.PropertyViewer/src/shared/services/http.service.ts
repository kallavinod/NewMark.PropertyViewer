import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private apiEndpoint: string='';

  constructor(private http: HttpClient, private configService: ConfigService) {    
  }

  getApiUri(): Observable<string> {
    return this.configService.loadConfig().pipe(
      map((config) => config.apiEndpoint) 
    );
  }
}

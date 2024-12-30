import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ConfigService } from '../../shared/services/config.service';
import { HttpService } from '../../shared/services/http.service';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-hierarchy',
  templateUrl: './hierarchy.component.html',
  styleUrls: ['./hierarchy.component.css']
})
export class HierarchyComponent implements OnInit {
  properties: any;
  selectedProperty: any;
  constructor(private configService: ConfigService,
    private apiService: HttpService,
    private http: HttpClient,
    private cdr: ChangeDetectorRef
  ) { }

  async ngOnInit() {
    setTimeout(() => {
       this.fetchData();
    }, 3000);
  }

  async  fetchData() {
    this.apiService.getApiUri().subscribe({
      next: (apiUri) => {
        this.http.get(`${apiUri}/Property`).subscribe({
          next: (result:any) => {
            console.log('API call successful. Result:', result);
            this.properties = result; // Store the result
            this.selectedProperty = this.properties[0];
            this.cdr.detectChanges();
          },
          error: (error) => {
            console.error('API call failed:', error);
          }
        });
      },
      error: (error) => {
        console.error('Failed to get API URI:', error);
      }
    });
  }

  
 

   // Display the first property by default

  selectProperty(property: any) {
    this.selectedProperty = property;
  }
}

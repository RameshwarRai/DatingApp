import { HttpClient, HttpRequest } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The Dating App';
  constructor(private http:HttpClient){}
  users:any;
  ngOnInit() {
   this.getUsers();
  }

  getUsers()
  {
    this.http.get('https://localhost:5000/api/users').subscribe(Response =>{
      this.users=Response;
    },error => {
      console.log(error);}
    );

  }

}

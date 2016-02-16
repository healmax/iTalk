iTalk
=
跨平台即時通訊方案

Demo
-
目前網頁初版已完成，部署於 Amazon Web Servces  
* 網站與 API 使用 Amazon EC2  
* 資料庫使用 Amazon RDS  

[開始 DEMO](http://italk-api.elasticbeanstalk.com/Admin/Home/Index)  

功能
-
*  建立帳戶  
  ![image](http://i.imgur.com/oNCvsQA.png)  
*  主畫面  
  ![image](http://i.imgur.com/nnovxGq.png)  
*  新增好友  
  ![image](http://i.imgur.com/u3gd0Qj.png)  
*  即時對話  
  ![image](http://i.imgur.com/ZuuKTqb.png)  
*  建立群組  
  ![image](http://i.imgur.com/VIiqRbv.png)  
*  群組細節  
  ![image](http://i.imgur.com/UoYhTtL.png)  
  
### 響應式網站設計
* PC  
  ![image](http://i.imgur.com/zQZBMtN.png)  
* Tablet  
  ![image](http://i.imgur.com/5fPiY6M.png)  
* Mobile  
  ![image](http://i.imgur.com/Jaw8rJ7.png)
  ![image](http://i.imgur.com/KOEriZi.png)  

使用技術
-
### 前端
* [jQuery](https://jquery.com/)  
* [jQuery Mobile](https://jquerymobile.com/)  
* [AngularJS](https://angularjs.org/)  
* [Angular Material](https://material.angularjs.org/latest/)  
* [Bootstrap](http://getbootstrap.com/)  
* [Font Awesome](https://fortawesome.github.io/Font-Awesome/)  
* [ASP.NET SignalR] (http://www.asp.net/signalr)  

為了打造跨裝置的 iTalk 網頁版，考慮即時通訊軟體的特性，前端採用了 Single Page Application 的概念開發。  
主要是透過 Ajax 以及 AngularJS 的 2-way binding，加上 SignalR 從後端即時推送新資料。  
使用 Bootstrap 與 AngularJS, Angular Material 將操作 DOM 物件的部分降到最低，加快開發速度。
jQuery Mobile 提供行動版網頁更好的使用者體驗

### 後端
* [ASP.NET MVC] (http://www.asp.net/mvc)  
* [ASP.NET Web API](http://www.asp.net/web-api)  
* [ASP.NET Identity](http://www.asp.net/identity)  
* [Entity Framework](https://msdn.microsoft.com/zh-tw/data/ef.aspx) Code First  
* [ImageResizer](http://imageresizing.net/)  

後端同時採用 ASP.NET Identity Cookie & Bearer Token 驗證，支持不同客戶端的 API 調用。  
使用 Task-based Asynchronous Pattern 處理 IO Bound 的工作，讓伺服器能處理更高的併發。

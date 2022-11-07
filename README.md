# ManagementCoach-.NET
Đồ án SE347
Ứng dụng bán mô hình đồ chơi Nhật Bản

## Yêu cầu
Ứng dụng này sử dụng postgresql để quản lý CSDL.

Tải postgresql tại đây: https://www.postgresql.org/download/ 

**!! Lưu ý khi cài postgresql, đặt username và password cho superuser là 'postgres' để ứng dụng có thể chạy được, nếu không thì phải tự sửa lại giá trị trong tag `<connectionStrings>` trong file 'App.config'!!**
 

## Setup
1. Clone repository

2. Cài dependency packages bằng cách build project

3. Thiết lập CSDL trên máy: 

    * Trong Visual Studio, mở Package Manager Console bằng cách vô View > Other Windows > Package Manager Console
    * Chạy lệnh `Update-Database`

4. Build và chạy ứng dụng

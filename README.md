# Student Management Software

A student management application for schools from high school level downwards, supporting the management of student information, teachers, classes, scores, and academic reports.

## 1. Team Members

| No. | Student ID | Full Name               | Class      |
|-----|------------|-------------------------|------------|
| 1   | 22520187   | Bùi Khánh Đang          | SE104.O24  |
| 2   | 22520190   | Nguyễn Lưu Minh Đăng    | SE104.O24  |
| 3   | 22520098   | Đặng Quốc Bảo           | SE104.O24  |
| 4   | 22520090   | Mai Thanh Bách          | SE104.O24  |
| 5   | 22520091   | Nguyễn Hoàng Bách       | SE104.O24  |

## 2. Purpose and Reasons for Choosing the Topic

### Purpose
- Build a comprehensive school management system, including student, teacher, class, score, and report management.
- Replace manual management methods using Excel/paperwork to improve accuracy and security.
- Provide an intuitive and user-friendly interface for administrators, teachers, and students.

### Reasons for Choosing the Topic
- Rapidly increasing student numbers make manual management complex and error-prone.
- Growing demand for digitizing educational management processes.

## 3. Key Features
- **Student Management**: Add, delete, edit information, and assign classes.
- **Teacher Management**: Assign teaching responsibilities, update information.
- **Class Management**: Create classes, manage student numbers, and timetables.
- **Score Management**: Input scores, calculate averages, and classify academic performance.
- **Reporting**: Statistical reports for semesters/subjects, export to Excel.
- **Authorization**: Role-based login (administrator, teacher, student).
- **Support**: Password recovery, password change, and personal information updates.

## 4. Technologies Used
- **Platform**: .NET Framework, Avalonia UI
- **Frontend**: C#, XAML
- **Backend**: Entity Framework, SQL Server
- **Libraries**: MaterialDesignXAML, SfChart, ExcelDataReader
- **Tools**: Visual Studio 2022, GitHub, Miro (UI design)

## 5. User Guide

<details>
  <summary>Login</summary>

| ![](./ReadmeAssets/Login.png) |
| :---------------------------: |
| _Login Screen_ |

1. Enter credentials.
2. Remember login.
3. Login.
4. Navigate to password recovery.

</details>

<details>
  <summary>Password Recovery</summary>

| ![](./ReadmeAssets/ForgotPassword.png) |
| :------------------------------------: |
| _Password Recovery Screen_ |

1. Enter information.
2. Send verification code via email.
3. Save new password.

</details>

<details>
  <summary>Edit Personal Information</summary>

| ![](./ReadmeAssets/PersonalInfo.png) |
| :----------------------------------: |
| _Personal Information Screen_ |

1. Enter information.
2. Open password change screen.
3. Save edited information.
4. Logout.

</details>

<details>
  <summary>Change Password</summary>

| ![](./ReadmeAssets/ChangePassword.png) |
| :----------------------------------: |
| _Personal Information Screen_ |

1. Enter information.
2. Save.
3. Cancel.

</details>

<details>
  <summary>Home Screen for Administrators/Teachers</summary>

| ![](./ReadmeAssets/Home_Admin.png) | ![](./ReadmeAssets/Home_Teacher.png) |
| :--------------------------------: | :----------------------------------: |
| _Administrator Home Screen_        | _Teacher Home Screen_ |

1. Display home screen.
2. Display reports.
3. Display information.
4. Display class management.
5. Display score management.
6. Display scoreboard system.
7. Display subject management.
8. Display student achievements.
9. Display teaching assignments.
10. Display regulation changes.
11. Display personal information.

</details>

<details>
  <summary>Classes management</summary>

| ![](./ReadmeAssets/Classes.png) |
| :----------------------------------: |
| _Classes Management Screen_ |

1. Filter combobox.
2. Open add class screen.
3. Search class name.
4. Open students in class screen.
5. Open edit class screen.
6. Remove class.

</details>

<details>
  <summary>Add/Edit Screen for Class</summary>

| ![](./ReadmeAssets/Class_Add.png) | ![](./ReadmeAssets/Class_Edit.png) |
| :--------------------------------: | :----------------------------------: |
| _Add Class Screen_        | _Edit Class Screen_ |

1. Enter/Edit information.
2. Save.
3. Cancel.

</details>

<details>
  <summary>Class Management</summary>

| ![](./ReadmeAssets/StudentsInClass.png) |
| :----------------------------------: |
| _Class Management Screen_ |

1. Search student in class.
2. Open add student to class screen.
3. Remove student from class. 

</details>

<details>
  <summary>Add Student to Class</summary>

| ![](./ReadmeAssets/StudentInClass_Add.png) |
| :----------------------------------: |
| _Add Student To Class Screen_ |

1. Search student.
2. Save/Confirm add student.
3. Cancel.

</details>

<details>
  <summary>Semester/Subject Grade Reports</summary>

| ![](./ReadmeAssets/Report_Semester.png) | ![](./ReadmeAssets/Report_Subject.png) |
| :--------------------------------: | :----------------------------------: |
| _Semester Grade Report Screen_        | _Subject Grade Report Screen_ |

1. Filter combobox.
2. Show all.

</details>

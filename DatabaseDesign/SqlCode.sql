/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2015/8/24 星期一 21:27:01                       */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('EntryRecord') and o.name = 'FK_ENTRY_RELATIONS_CREATOR')
alter table EntryRecord
   drop constraint FK_ENTRY_RELATIONS_CREATOR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('EntryRecord') and o.name = 'FK_ENTRY_RELATIONS_HANDLER')
alter table EntryRecord
   drop constraint FK_ENTRY_RELATIONS_HANDLER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('EntryRecord') and o.name = 'FK_ENTRY_RELATIONS_FLOW')
alter table EntryRecord
   drop constraint FK_ENTRY_RELATIONS_FLOW
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FlowProcess') and o.name = 'FK_FLOW_RELATIONS_USER')
alter table FlowProcess
   drop constraint FK_FLOW_RELATIONS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FlowTask') and o.name = 'FK_TASK_RELATIONS_PROCESS')
alter table FlowTask
   drop constraint FK_TASK_RELATIONS_PROCESS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('FlowTask') and o.name = 'FK_TASK_RELATIONS_APPROVER')
alter table FlowTask
   drop constraint FK_TASK_RELATIONS_APPROVER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SecurityUser') and o.name = 'FK_USER_RELATIONS_ROLE')
alter table SecurityUser
   drop constraint FK_USER_RELATIONS_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SystemMenu') and o.name = 'FK_MENU_RELATIONS_MENUPAR')
alter table SystemMenu
   drop constraint FK_MENU_RELATIONS_MENUPAR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SystemRight') and o.name = 'FK_RIGHT_RELATIONS_MENU')
alter table SystemRight
   drop constraint FK_RIGHT_RELATIONS_MENU
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SystemRight') and o.name = 'FK_RIGHT_RELATIONS_USER')
alter table SystemRight
   drop constraint FK_RIGHT_RELATIONS_USER
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('EntryRecord')
            and   name  = 'Relationship_9_FK'
            and   indid > 0
            and   indid < 255)
   drop index EntryRecord.Relationship_9_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('EntryRecord')
            and   name  = 'Relationship_8_FK'
            and   indid > 0
            and   indid < 255)
   drop index EntryRecord.Relationship_8_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('EntryRecord')
            and   name  = 'Relationship_7_FK'
            and   indid > 0
            and   indid < 255)
   drop index EntryRecord.Relationship_7_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EntryRecord')
            and   type = 'U')
   drop table EntryRecord
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FlowProcess')
            and   name  = 'Relationship_12_FK'
            and   indid > 0
            and   indid < 255)
   drop index FlowProcess.Relationship_12_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FlowProcess')
            and   type = 'U')
   drop table FlowProcess
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FlowTask')
            and   name  = 'Relationship_6_FK'
            and   indid > 0
            and   indid < 255)
   drop index FlowTask.Relationship_6_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('FlowTask')
            and   name  = 'Relationship_4_FK'
            and   indid > 0
            and   indid < 255)
   drop index FlowTask.Relationship_4_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('FlowTask')
            and   type = 'U')
   drop table FlowTask
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SecurityRole')
            and   type = 'U')
   drop table SecurityRole
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SecurityUser')
            and   name  = 'Relationship_10_FK'
            and   indid > 0
            and   indid < 255)
   drop index SecurityUser.Relationship_10_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SecurityUser')
            and   type = 'U')
   drop table SecurityUser
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SystemMenu')
            and   name  = 'Relationship_11_FK'
            and   indid > 0
            and   indid < 255)
   drop index SystemMenu.Relationship_11_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemMenu')
            and   type = 'U')
   drop table SystemMenu
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SystemRight')
            and   name  = 'Relationship_3_FK'
            and   indid > 0
            and   indid < 255)
   drop index SystemRight.Relationship_3_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('SystemRight')
            and   name  = 'Relationship_2_FK'
            and   indid > 0
            and   indid < 255)
   drop index SystemRight.Relationship_2_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SystemRight')
            and   type = 'U')
   drop table SystemRight
go

/*==============================================================*/
/* Table: EntryRecord                                           */
/*==============================================================*/
create table EntryRecord (
   RecordID             bigint               identity,
   CreatorName          varchar(50)          not null,
   CompleterName        varchar(50)          null,
   ProcessID            bigint               not null,
   CreationTime         datetime             not null,
   CompletionTime       datetime             null,
   ProjectName          varchar(100)         not null,
   ProjectNum           varchar(100)         not null,
   ProductName          varchar(100)         not null,
   SuiteCount           int                  not null,
   ProductImgNum        varchar(100)         not null,
   Sequence             varchar(100)         not null,
   ImgNum               varchar(100)         not null,
   Height               decimal(18,8)        not null,
   Width                decimal(18,8)        not null,
   StomachWeight        decimal(18,8)        not null,
   WingWeight           decimal(18,8)        not null,
   Length               decimal(18,8)        not null,
   PieceCount           int                  not null,
   Weight               decimal(18,8)        not null,
   AssemblageDate       datetime             not null,
   SolderingDate        datetime             not null,
   CorrectionDate       datetime             not null,
   InspectionDate       datetime             not null,
   CompletionDate       datetime             null,
   DespatchDate         datetime             null,
   constraint PK_ENTRYRECORD primary key nonclustered (RecordID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '入库记录',
   'user', @CurrentUser, 'table', 'EntryRecord'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'RecordID',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'RecordID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建人',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CreatorName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '完成人',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CompleterName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ProcessID',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ProcessID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '录入时间',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CreationTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '完成时间',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CompletionTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '项目名称',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ProjectName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '项目工号',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ProjectNum'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '产品名称',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ProductName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '套数',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'SuiteCount'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '产品图号',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ProductImgNum'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '序号',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'Sequence'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '图号',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'ImgNum'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '高度',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'Height'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '宽度',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'Width'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '腹厚',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'StomachWeight'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '翼厚',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'WingWeight'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '长度',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'Length'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '件数',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'PieceCount'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '单重',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'Weight'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '组立日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'AssemblageDate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '焊接日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'SolderingDate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '矫正日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CorrectionDate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '报检日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'InspectionDate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '完工日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'CompletionDate'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '发运日期',
   'user', @CurrentUser, 'table', 'EntryRecord', 'column', 'DespatchDate'
go

/*==============================================================*/
/* Index: Relationship_7_FK                                     */
/*==============================================================*/
create index Relationship_7_FK on EntryRecord (
CreatorName ASC
)
go

/*==============================================================*/
/* Index: Relationship_8_FK                                     */
/*==============================================================*/
create index Relationship_8_FK on EntryRecord (
CompleterName ASC
)
go

/*==============================================================*/
/* Index: Relationship_9_FK                                     */
/*==============================================================*/
create index Relationship_9_FK on EntryRecord (
ProcessID ASC
)
go

/*==============================================================*/
/* Table: FlowProcess                                           */
/*==============================================================*/
create table FlowProcess (
   ProcessID            bigint               identity,
   CurrentHandler       varchar(50)          null,
   Name                 varchar(200)         not null,
   SetupTime            datetime             not null,
   CompleteTime         datetime             null,
   Status               int                  not null,
   CurrentTaskSection   varchar(100)         null,
   CurrentHandleUrl     varchar(500)         not null,
   constraint PK_FLOWPROCESS primary key nonclustered (ProcessID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '流程',
   'user', @CurrentUser, 'table', 'FlowProcess'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ProcessID',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'ProcessID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前处理人',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'CurrentHandler'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '流程名称',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '启动时间',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'SetupTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '完成时间',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'CompleteTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '状态',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'Status'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前任务节点',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'CurrentTaskSection'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '当前处理Url',
   'user', @CurrentUser, 'table', 'FlowProcess', 'column', 'CurrentHandleUrl'
go

/*==============================================================*/
/* Index: Relationship_12_FK                                    */
/*==============================================================*/
create index Relationship_12_FK on FlowProcess (
CurrentHandler ASC
)
go

/*==============================================================*/
/* Table: FlowTask                                              */
/*==============================================================*/
create table FlowTask (
   TaskID               bigint               identity,
   ProcessID            bigint               not null,
   HandlerName          varchar(50)          not null,
   SectionName          varchar(100)         not null,
   Name                 varchar(200)         not null,
   HandleTime           datetime             not null,
   HandleUrl            varchar(500)         not null,
   constraint PK_FLOWTASK primary key nonclustered (TaskID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '任务',
   'user', @CurrentUser, 'table', 'FlowTask'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'TaskID',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'TaskID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ProcessID',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'ProcessID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '处理人',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'HandlerName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '任务节点',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'SectionName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '任务名称',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'Name'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '处理时间',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'HandleTime'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '处理Url',
   'user', @CurrentUser, 'table', 'FlowTask', 'column', 'HandleUrl'
go

/*==============================================================*/
/* Index: Relationship_4_FK                                     */
/*==============================================================*/
create index Relationship_4_FK on FlowTask (
ProcessID ASC
)
go

/*==============================================================*/
/* Index: Relationship_6_FK                                     */
/*==============================================================*/
create index Relationship_6_FK on FlowTask (
HandlerName ASC
)
go

/*==============================================================*/
/* Table: SecurityRole                                          */
/*==============================================================*/
create table SecurityRole (
   RoleID               int                  identity,
   RoleName             varchar(50)          not null,
   Enabled              bit                  not null,
   constraint PK_SECURITYROLE primary key nonclustered (RoleName)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '角色',
   'user', @CurrentUser, 'table', 'SecurityRole'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'RoleID',
   'user', @CurrentUser, 'table', 'SecurityRole', 'column', 'RoleID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色名称',
   'user', @CurrentUser, 'table', 'SecurityRole', 'column', 'RoleName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Enabled',
   'user', @CurrentUser, 'table', 'SecurityRole', 'column', 'Enabled'
go

/*==============================================================*/
/* Table: SecurityUser                                          */
/*==============================================================*/
create table SecurityUser (
   UserID               int                  identity,
   UserName             varchar(50)          not null,
   RoleName             varchar(50)          null,
   DisplayName          varchar(50)          not null,
   Password             varchar(50)          not null,
   Enabled              bit                  not null,
   CreationTime         datetime             not null,
   constraint PK_SECURITYUSER primary key nonclustered (UserName)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '用户',
   'user', @CurrentUser, 'table', 'SecurityUser'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户ID',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'UserID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户名',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'UserName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色名称',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'RoleName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户姓名',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'DisplayName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '密码',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'Password'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Enabled',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'Enabled'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'SecurityUser', 'column', 'CreationTime'
go

/*==============================================================*/
/* Index: Relationship_10_FK                                    */
/*==============================================================*/
create index Relationship_10_FK on SecurityUser (
RoleName ASC
)
go

/*==============================================================*/
/* Table: SystemMenu                                            */
/*==============================================================*/
create table SystemMenu (
   MenuID               int                  identity,
   MenuName             varchar(100)         not null,
   ParentMenuName       varchar(100)         null,
   Enabled              bit                  not null,
   constraint PK_SYSTEMMENU primary key nonclustered (MenuName)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '系统菜单',
   'user', @CurrentUser, 'table', 'SystemMenu'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'MenuID',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'MenuID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '菜单名称',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'MenuName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '父级菜单名称',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'ParentMenuName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Enabled',
   'user', @CurrentUser, 'table', 'SystemMenu', 'column', 'Enabled'
go

/*==============================================================*/
/* Index: Relationship_11_FK                                    */
/*==============================================================*/
create index Relationship_11_FK on SystemMenu (
ParentMenuName ASC
)
go

/*==============================================================*/
/* Table: SystemRight                                           */
/*==============================================================*/
create table SystemRight (
   RightID              int                  identity,
   MenuName             varchar(100)         not null,
   RoleName             varchar(50)          not null,
   Enabled              bit                  not null,
   constraint PK_SYSTEMRIGHT primary key nonclustered (RightID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sys.sp_addextendedproperty 'MS_Description', 
   '系统权限',
   'user', @CurrentUser, 'table', 'SystemRight'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'RightID',
   'user', @CurrentUser, 'table', 'SystemRight', 'column', 'RightID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '菜单名称',
   'user', @CurrentUser, 'table', 'SystemRight', 'column', 'MenuName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色名称',
   'user', @CurrentUser, 'table', 'SystemRight', 'column', 'RoleName'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Enabled',
   'user', @CurrentUser, 'table', 'SystemRight', 'column', 'Enabled'
go

/*==============================================================*/
/* Index: Relationship_2_FK                                     */
/*==============================================================*/
create index Relationship_2_FK on SystemRight (
MenuName ASC
)
go

/*==============================================================*/
/* Index: Relationship_3_FK                                     */
/*==============================================================*/
create index Relationship_3_FK on SystemRight (
RoleName ASC
)
go

alter table EntryRecord
   add constraint FK_ENTRY_RELATIONS_CREATOR foreign key (CreatorName)
      references SecurityUser (UserName)
go

alter table EntryRecord
   add constraint FK_ENTRY_RELATIONS_HANDLER foreign key (CompleterName)
      references SecurityUser (UserName)
go

alter table EntryRecord
   add constraint FK_ENTRY_RELATIONS_FLOW foreign key (ProcessID)
      references FlowProcess (ProcessID)
go

alter table FlowProcess
   add constraint FK_FLOW_RELATIONS_USER foreign key (CurrentHandler)
      references SecurityUser (UserName)
go

alter table FlowTask
   add constraint FK_TASK_RELATIONS_PROCESS foreign key (ProcessID)
      references FlowProcess (ProcessID)
go

alter table FlowTask
   add constraint FK_TASK_RELATIONS_APPROVER foreign key (HandlerName)
      references SecurityUser (UserName)
go

alter table SecurityUser
   add constraint FK_USER_RELATIONS_ROLE foreign key (RoleName)
      references SecurityRole (RoleName)
go

alter table SystemMenu
   add constraint FK_MENU_RELATIONS_MENUPAR foreign key (ParentMenuName)
      references SystemMenu (MenuName)
go

alter table SystemRight
   add constraint FK_RIGHT_RELATIONS_MENU foreign key (MenuName)
      references SystemMenu (MenuName)
go

alter table SystemRight
   add constraint FK_RIGHT_RELATIONS_USER foreign key (RoleName)
      references SecurityRole (RoleName)
go


-- 通知表
delete from notice n
where n.UpdateTime IS NOT NULL 
	AND DATEDIFF(CURRENT_DATE(),n.UpdateTime) >90

-- 接收通知人员表
delete from noticereceiver ne 
where ne.PublishTime IS NOT NULL
	AND DATEDIFF(CURRENT_DATE(),ne.PublishTime) >90

-- 通知接受人员状态表
delete from receiverstatus re 
where re.ReceiveTime IS NOT NULL
  AND DATEDIFF(CURRENT_DATE(),re.ReceiveTime) >90

-- 操作日志表--OperateType(1:登录,5:退出)
select * from log o 
where o.OperateTime IS NOT NULL 
	AND DATEDIFF(CURRENT_DATE(),o.OperateTime) > 90

-- 值班日志表
select * from dutylog d 
where d.StartTime IS NOT NULL 
	AND  DATEDIFF(CURRENT_DATE(),d.StartTime) > 90

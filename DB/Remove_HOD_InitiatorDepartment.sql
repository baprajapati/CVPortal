DELETE from VendorApproval where CreatedById in (select UserId from webpages_UsersInRoles where RoleId 
	in (select RoleId from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment')))

DELETE from CustomerApproval where CreatedById in (select UserId from webpages_UsersInRoles where RoleId 
	in (select RoleId from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment')))

DELETE from tbl_Users where Id in (select UserId from webpages_UsersInRoles where RoleId 
	in (select RoleId from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment')))

DELETE from webpages_UsersInRoles where RoleId 
	in (select RoleId from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment'))

DELETE from webpages_Membership where UserId in (select UserId from webpages_UsersInRoles where RoleId 
	in (select RoleId from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment')))

DELETE from webpages_Roles where RoleName in ('HODDepartment','InitiatorDepartment')

UPDATE webpages_Roles SET RoleName = 'Initiator' where RoleName = 'InitiatorAdmin'
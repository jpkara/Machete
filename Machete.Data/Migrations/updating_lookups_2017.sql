/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
  FROM [dbo].[Lookups]
  where category = 'transportmethod'

update lookups set [key] = 'skill_general_labor' where id = 60
update lookups set [key] = 'skill_painting_rollerbrush' where id = 61
update lookups set [key] = 'skill_painting_spray' where id = 62
update lookups set [key] = 'skill_drywall' where id = 63
update lookups set [key] = 'skill_landscaping_fence' where id = 64
update lookups set [key] = 'skill_carpentry' where id = 65
update lookups set [key] = 'skill_masonry' where id = 66
update lookups set [key] = 'skill_deep_cleaning' where id = 67
update lookups set [key] = 'skill_moving' where id = 68
update lookups set [key] = 'skill_yardwork' where id = 69
update lookups set [key] = 'skill_basic_cleaning' where id = 74
update lookups set [key] = 'skill_demolition' where id = 77
update lookups set [key] = 'skill_adv_gardening' where id = 83
update lookups set [key] = 'skill_landscaping' where id = 88
update lookups set [key] = 'skill_roofing' where id = 89
update lookups set [key] = 'skill_event_help' where id = 118
update lookups set [key] = 'skill_pressure_washing' where id = 120
update lookups set [key] = 'skill_digging' where id = 122
update lookups set [key] = 'skill_hauling' where id = 128
update lookups set [key] = 'skill_insulation' where id = 131
update lookups set [key] = 'skill_tile_install' where id = 132
update lookups set [key] = 'skill_drywall' where id = 133
update lookups set [key] = 'skill_flooring' where id = 183


update lookups set [key] = 'transport_bus' where id = 29
update lookups set [key] = 'transport_car' where id = 30
update lookups set [key] = 'transport_pickup' where id = 31
update lookups set [key] = 'transport_van' where id = 32
update lookups set [key] = 'transport_walks' where id = 86

update configs set publicConfig = 0 where category = 'Emails'
update configs set publicConfig = 0 where [key] = 'PayPalAccountID'

select * from configs

select * from employers where email = 'jciispam@gmail.com'
update employers set onlineSigninID = '1F6D3643-B6DE-4951-A0A6-F63740D45A46' where email = 'jciispam@gmail.com' 
delete from employers where id = 46770
select * from employers where onlineSigninId = '1F6D3643-B6DE-4951-A0A6-F63740D45A46'

truncate table reportdefinitions
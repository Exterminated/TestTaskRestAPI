Описание web-сервиса для определения географического местоположения пользователя по IP-адресу.

Сревис состоит из двух частей:
	- UpdateDBConsoleApp
		Консольное приложение для обновления Postgres базы с хранилищем IP адресов и геолокаций
	- IpFinderREST
		Веб-служба для получения географической информации по GET запросу IP адреса из базы
		
Для функционирования сервисов нужен PostgreSQL

UpdateDBConsoleApp
	- Target framework: .NET Core 3.1
	- Entity Framework Core.PostgreSQL 3.1.0
	- Microsoft.Extensions.Configuration 3.1.0

	Приложение реализовано по принципу CodeFirst на EF. Для начала функционирования необходимо создать базу и пользователя для нее в субд PostgreSQL. По умолчанию, настроено на БД IPsDB и пользователя TestUser.
	
	!!Импорт большого количества записей (2-3 миллиона) может занимать продолжительное время!!
	
	В качестве источника данных по IP, приложение использует GeoLite2 от MaxMind. Приложение настроено на импорт данных типа GeoLite2-City-CSV. 
	
	Для корректной работы приложению нужны два csv файла:
		- GeoLite2-City-Blocks-IPv4.csv
			Источник данных по IP адресам
		- GeoLite2-City-Locations-en.csv
			Источник данных по географическим положениям
	
	Для удобства все настрйоки прилжения хранятся в конфигурационном файле appsettings.json
	
	ConnectionStrings - группа строк подключения к серверу SQL
		DefaultConnection - подключение по умолчанию к SQL серверу
	ImportingData - группа строк для импорта csv	
		ipv4_csv - расположние файла импорта IPv4 адресов (файл GeoLite2-City-Blocks-IPv4.csv)
		loc_csv - расположение файла импорта географических названий (файл GeoLite2-City-Locations-en.csv)
	
IpFinderREST
	- Target framework: .NET Core 3.1
	- Entity Framework Core.PostgreSQL 3.1.0
	- Microsoft.Extensions.Configuration 3.1.0
	
	Приложение реализовано по принципу CodeFirst на EF. Для начала функционирования необходимо создать базу и пользователя для нее в субд PostgreSQL. По умолчанию, настроено на БД IPsDB и пользователя TestUser.
	
	По умолчанию подключено анонимное подключение, а также использование SSL на порт 44344.
	
	Для удобства все настрйоки прилжения хранятся в конфигурационном файле launchSettings.json в Properties
		ConnectionStrings - группа строк подключения к серверу SQL
			DefaultConnection - подключение по умолчанию к SQL серверу
	
	Для получения инхормации по IP адресу необходимо выполнить запрос {application URL}/api/GeoIp/{IP},	где {IP} - валидный IPv4 адрес.
	
	Ответ - json файл следующей структуры:	
		
		iP_Info	
			ipId			
			network			
			geoname_id		
			registered_country_geoname_id	
			represented_country_geoname_id	
			is_anonymous_proxy	
			is_satellite_provider	
			postal_code	
			latitude	
			longitude	
			accuracy_radius	
		geo_Info	
			cityId	
			geoname_id	
			locale_code	
			continent_code	
			continent_name	
			country_iso_code	
			country_name	
			subdivision_1_iso_code	
			subdivision_1_name	
			subdivision_2_iso_code	
			subdivision_2_name	
			city_name	
			metro_code	
			time_zone	
			is_in_european_union

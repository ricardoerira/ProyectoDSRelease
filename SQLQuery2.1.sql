select * from HojaVidas where primer_apellido like 'ruiz'
select * from HojaVidas where primer_apellido like 'silva'

select * from estudiantes where estudianteid  = 21
select * from docentes where docenteid  = 815
select * from Rotacions where rotacionid =35

select * from RotacionEstudiantes where rotacionid =35
select * from HojaVidas where hojavidaid  = 1346

delete  from hojavidas where hojavidaid in (select DISTINCT hojavidaid from HojaVidas h where h.hojaVidaId not in (select hojavidaid from estudiantes) and h.hojaVidaId not in (select hojavidaid from docentes)) 
select  DISTINCT hojavidaid from HojaVidas h where h.hojaVidaId not in (select hojavidaid from estudiantes) and h.hojaVidaId not in (select hojavidaid from docentes) ;


select  rotacionId,estudianteId from RotacionEstudiantes where rotacionid=35 group by  rotacionId,estudianteId


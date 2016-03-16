
select * from estudiantes where codigo= 521122328
select * from docentes where docenteid= 594


select * from docentes where num_documento = 75068440


select codigo from estudiantes group by codigo having count(codigo)>1
select Count(*) from docentes
select Count(*) from estudiantes
select Count(*) from rotacions
select Count(*) from rotacionEstudiantes


select * from hojaVidas where primer_apellido= 'sanabria'



select * from estudiantes where hojavidaid= 1614

select * from hojaVidas where hojavidaid= 2250

update RotacionDocentes  set nombre=h.primer_nombre  from HojaVidas h, Docentes d where RotacionDocentes.docenteid= d.docenteid   
 and  h.hojavidaid = d.hojaVidaid 


DELETE Estudiantes
FROM Estudiantes
LEFT OUTER JOIN (
   SELECT MIN(estudianteId) as estudianteId, codigo 
   FROM Estudiantes
   GROUP BY codigo 
) as KeepRows ON
   Estudiantes.estudianteId = KeepRows.estudianteId
WHERE
   KeepRows.estudianteId IS NULL


select * from RotacionEstudiantes where rotacionestudianteid=396

select * from Rotacions where rotacionid= 1


select count(*) from RotacionEstudiantes


select * from RotacionEstudiantes where rotacionid =98

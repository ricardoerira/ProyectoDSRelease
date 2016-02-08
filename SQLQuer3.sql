DELETE Docentes 
FROM Docentes
LEFT OUTER JOIN (
   SELECT MIN(docenteId) as docenteId, num_documento 
   FROM Docentes 
   GROUP BY num_documento
) as KeepRows ON
   Docentes.docenteId = KeepRows.docenteId
WHERE
   KeepRows.docenteId IS NULL
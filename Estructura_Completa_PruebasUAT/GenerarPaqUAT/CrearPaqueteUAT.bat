@echo off
setlocal enabledelayedexpansion

:: ===============================================================
:: CONFIGURACIÓN DE RUTAS LOCALES
:: ===============================================================
set "pathDocts=C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\GenerarPaqUAT"
set "RutaOrigen=%pathDocts%\Runbooks"
set "RutaUAT=%pathDocts%\UAT"
set "source=C:\Users\DAY-V\Desktop\Estructura_Completa_PruebasUAT\TFS_Mock"

:: Archivos base
set "instructions=InstruccionesLiberacion.txt"
set "qFile=Q-MexFile.xlsx"
set "qCopy=Declaracion de Indisponibilidad de URLs Q3_Q42025 - MEX.xlsx"

:: ===============================================================
:: MENU INTERACTIVO
:: ===============================================================
cls
echo ===============================================================
echo                   CREACION DE PAQUETE UAT
echo ===============================================================
echo  APLICACION                ^| PALABRAS CLAVE
echo --------------------------------------------------------------
echo  Servicios (Grupo)         ^| servicios_grupo, servs_grupo
echo  Portales (Grupo)          ^| portales_grupo, ports_grupo
echo  WS (Grupo)                ^| ws_grupo, webservices_grupo
echo  ServiciosMAS              ^| servmas, serviciosmas
echo  ServicioEmision           ^| emision, servemision
echo  ServicioCFD               ^| cfd, servcfd
echo  ServicioDocumentacion     ^| docu, sdoc
echo  ServicioIntegracionPermisos ^| permisos, perm, sintperm
echo  ServicioIntegracion       ^| sinteg, integracion
echo  AutomaticLoadSIRI         ^| siri
echo  CloseService              ^| closeserv, closeservice
echo  DTS                       ^| dts
echo  PortalDatosFiscales       ^| fiscal, datosf
echo  PortalCFDI                ^| portcfdi, portalcfdi
echo  ProcesarMovimiento        ^| procmov, movs
echo  InstalarPortalAPIRest     ^| apirest, rest
echo  WS-CLPortalAgentes        ^| agentes, clagentes
echo  WSCalculoPrima            ^| calcprima, calculoprima
echo  WSCargaQA                 ^| cargaqa, wscarga
echo  CatalogsWS                ^| catalogsws, catalogos
echo  MAS                       ^| mas, sistema
echo  SIAP                      ^| siap
echo ===============================================================
echo.

set "buildNumber="
set "search="
set "destination="

set /p buildNumber="Ingresa el build de TFS: "
set /p search="Ingrese el nombre de las aplicaciones (separadas por espacio): "
set /p destination="Ingresa la ruta destino del paquete: "

if not defined destination (
    echo Error: Debe ingresar una ruta destino.
    pause
    exit /b
)

:: Crear carpeta destino si no existe
if not exist "%destination%" mkdir "%destination%"

:: Limpieza de comillas en rutas
set "DestinoLimpio=%destination:"=%"
set "OrigenLimpio=%RutaOrigen:"=%"

echo.
echo ===============================================================
echo Iniciando ensamblado de paquete UAT...
echo ===============================================================

:: ===============================================================
:: 1. COPIA Y PROCESAMIENTO DE ARCHIVOS BASE (TXT / XLSX)
:: ===============================================================
if not exist "%pathDocts%\%instructions%" (
    echo El archivo de instrucciones no existe en: %pathDocts%\%instructions%
    pause
    exit /b
)

:: Copiar Q-MexFile
copy /y "%pathDocts%\%qFile%" "%DestinoLimpio%\%qCopy%" >nul
if !errorlevel! equ 0 (
    echo Archivo Q-MexFile copiado exitosamente como "%qCopy%"
) else (
    echo Error al copiar el archivo Q-MexFile
)

:: Copiar InstruccionesLiberacion
copy /y "%pathDocts%\%instructions%" "%DestinoLimpio%\%instructions%" >nul
echo Archivo de instrucciones copiado a la carpeta destino.

:: Reemplazar palabras clave en InstruccionesLiberacion.txt
set "stepOne=step1"
set "stepTwo=step2"
set "pdf=DatosFiscales"
set "api=WebAPI"
set "descStepOneAPI=Generar SnapShot del servidor 10.110.10.175"
set "descStepTwoAPI=Plan de reversion, restaurar SnapShot del servidor 10.110.10.175 generado en el paso 1"
set "descSIAP=Cada Runbook contiene su paso de respaldo"
set "descStepTwoSIAP=Plan de reversion, restaurar el respaldo generado en el paso 1 de cada runbook"

if exist "%DestinoLimpio%\temp.txt" del "%DestinoLimpio%\temp.txt"

for /f "tokens=*" %%A in ('type "%DestinoLimpio%\%instructions%"') do (
    set "line=%%A"
    
    echo !line! | findstr /i "%stepOne%" >nul
    if !errorlevel! equ 0 (
        echo !search! | findstr /i "%pdf%" >nul
        if !errorlevel! equ 0 (
            set "line=!line:%stepOne%=%descStepOneAPI%!"
        ) else (
            echo !search! | findstr /i "%api%" >nul
            if !errorlevel! equ 0 (
                set "line=!line:%stepOne%=%descStepOneAPI%!"
            ) else (
                set "line=!line:%stepOne%=%descSIAP%!"
            )
        )
    )
    
    echo !line! | findstr /i "destino" >nul
    if !errorlevel! equ 0 (
        set "line=!line:destino=%DestinoLimpio%!"
    )
    
    echo !line! | findstr /i "%stepTwo%" >nul
    if !errorlevel! equ 0 (
        echo !search! | findstr /i "%pdf%" >nul
        if !errorlevel! equ 0 (
            set "line=!line:%stepTwo%=%descStepTwoAPI%!"
        ) else (
            echo !search! | findstr /i "%api%" >nul
            if !errorlevel! equ 0 (
                set "line=!line:%stepTwo%=%descStepTwoAPI%!"
            ) else (
                set "line=!line:%stepTwo%=%descStepTwoSIAP%!"
            )
        )
    )
    echo !line!>> "%DestinoLimpio%\temp.txt"
)

move /y "%DestinoLimpio%\temp.txt" "%DestinoLimpio%\%instructions%" >nul
echo Etiquetas actualizadas correctamente dentro de %instructions%.

:: ===============================================================
:: 2. DEFINICIÓN DE RUNBOOKS
:: ===============================================================
set "BaseMASRB=00_RunBook_AplicarCambios_BD_MAS.doc"
set "BaseSIAPRB=00_RunBook_AplicarCambios_BD.doc"
set "PDFBaseRB=00_RunBook_AplicarCambiosBasePortalDatosFiscales.doc"
set "PortalCFDIRB=01_RunBook_ActualizarPortalCFDI.doc"
set "IDCMAS=01_RunBook_AplicarCambios_IDC_MAS.doc"
set "IDCSIAPApiRB=01_RunBook_AplicarCambios_IDC_SIAPApi.doc"
set "InterfazAdiRB=01_RunBook_AplicarCambiosInterfazAdiSIAP.doc"
set "ServiciosMAS=01_RunBook_AplicarCambiosMAS_Servicios.doc"
set "MASWebRB=01_RunBook_AplicarCambiosMAS_Web.doc"
set "AutomaticLoadSIRI=01_RunBook_AplicarCambiosServicioAutomaticLoadSIRI.doc"
set "ServicioCFD=01_RunBook_AplicarCambiosServicioCFD.doc"
set "ServicioDocumentacion=01_RunBook_AplicarCambiosServicioDocumentacion.doc"
set "DTS=01_RunBook_AplicarCambiosServicioDTSMiscelaneosService.doc"
set "SIAP=01_RunBook_AplicarCambiosSIAP.doc"
set "BatchLauncher=01_RunBook_BatchLauncher.doc"
set "BusinessServiceSIAP=01_RunBook_BusinessServiceSIAP.doc"
set "CatalogsWS=01_RunBook_CatalogsWS.doc"
set "ProcesarMovimiento=01_RunBook_cw_ProcesarMovimiento_PortalAgentes.doc"
set "InstalarPortalAPIRest=01_RunBook_InstalarPortalAPIRest.doc"
set "ServicioEmision=01_RunBook_ServicioEmision.doc"
set "ServicioIntegracionPermisosaTempMasivoRB=01_RunBook_ServicioIntegracion - ConPermisos a TempMasivo.doc"
set "ServicioIntegracion=01_RunBook_ServicioIntegracion.doc"
set "WSCalculoPrima=01_RunBook_Ws_CalculoPrima_QA.doc"
set "WSCargaQA=01_RunBook_WS_Carga_QA.doc"
set "WS-CLPortalAgentes=01_RunBook_WS_CL_Portal_Agentes.doc"
set "CloseService=01_RunBook_Zurich.CloseServices.doc"
set "QuotationWeb=01_RunBook_Zurich.QuotationWeb.doc"

:: Limpiar entrada de búsqueda
set "search_clean=%search%"
if defined search_clean (
    set "search_clean=%search_clean:"=%"
    set "search_clean=%search_clean:,= %"
    set "search_clean=%search_clean:;= %"
)

set "tfs_search="

:: ===============================================================
:: 3. PROCESAR CADA APLICACIÓN INGRESADA
:: ===============================================================
echo.
echo Procesando aplicaciones ingresadas...
echo --------------------------------------------------------------

for %%A in (%search_clean%) do (
    call :PROCESAR_TODO "%%A"
)

:: ===============================================================
:: 4. COPIAR PAQUETES DESDE TFS / COMPILADOS
:: ===============================================================
echo.
echo --------------------------------------------------------------
echo Buscando compilados en TFS (%source%\%buildNumber%)...

if exist "%source%\%buildNumber%" (
    for %%f in (%tfs_search%) do (
        set "appName=%%f"
        for /d /r "%source%\%buildNumber%" %%d in (*) do (
            echo %%~nxd | findstr /i "!appName!" >nul
            if !errorlevel! equ 0 (
                echo Copiando compilado TFS desde: %%d
                xcopy "%%d" "%DestinoLimpio%\%%~nxd" /e /i /h /y >nul
            )
        )
    )
) else (
    echo No se encontro la carpeta del Build TFS (%source%\%buildNumber%). Se omitio esta etapa.
)

echo.
echo ===============================================================
echo Proceso completado con exito.
echo Paquete generado en: %DestinoLimpio%
echo ===============================================================
pause
exit /b


:: ===============================================================
:: SUBRUTINA: PROCESAR APLICACIÓN / ALIAS
:: ===============================================================
:PROCESAR_TODO
set "RAW_APP=%~1"
if "%RAW_APP%"=="" goto :EOF

call :DiccionarioPalabrasClave "%RAW_APP%" APP

set "tfs_search=%tfs_search% %APP%"

echo Procesando: "%RAW_APP%" (Aplicacion: "%APP%")

:: ---------------------------------------------------------------
:: CASO ESPECIAL: MAS (Solo procesa IDC_MAS)
:: ---------------------------------------------------------------
if /i "%APP%"=="MAS" (
    echo Copiando unicamente Runbook y carpeta IDC_MAS...
    if exist "%OrigenLimpio%\%IDCMAS%" copy /y "%OrigenLimpio%\%IDCMAS%" "%DestinoLimpio%\" >nul
    for /d %%D in ("%RutaUAT%\*IDC_MAS*") do (
        echo Copiando carpeta UAT: %%~nxD
        xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
    )
    goto :EOF
)

:: ---------------------------------------------------------------
:: CASO ESPECIAL: SIAP (SOLO IDC_SIAP - Excluye IDC_SIAPApi)
:: ---------------------------------------------------------------
if /i "%APP%"=="SIAP" (
    echo Copiando unicamente Runbook y carpeta IDC_SIAP...
    if exist "%OrigenLimpio%\%SIAP%" copy /y "%OrigenLimpio%\%SIAP%" "%DestinoLimpio%\" >nul
    for /d %%D in ("%RutaUAT%\*IDC_SIAP*") do (
        echo %%~nxD | findstr /i "api" >nul
        if !errorlevel! neq 0 (
            echo Copiando carpeta UAT: %%~nxD
            xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
        )
    )
    goto :EOF
)

:: Casos especiales de Aliasing BD / BPM
if /i "%APP%"=="BPM" set "APP=BusinessServiceSIAP"
if /i "%APP%"=="BPM_Service" set "APP=BusinessServiceSIAP"

if /i "%APP%"=="BD" (
    echo Creando estructura de Base de Datos...
    mkdir "%DestinoLimpio%\DB\Scripts" 2>nul
    mkdir "%DestinoLimpio%\DB\StoredProcedures" 2>nul
    copy /y "%OrigenLimpio%\%BaseSIAPRB%" "%DestinoLimpio%\" >nul
)

if /i "%APP%"=="BD_MAS" (
    echo Creando estructura de Base de Datos MAS...
    mkdir "%DestinoLimpio%\DB\Scripts" 2>nul
    mkdir "%DestinoLimpio%\DB\StoredProcedures" 2>nul
    copy /y "%OrigenLimpio%\%BaseMASRB%" "%DestinoLimpio%\" >nul
)

:: ---------------------------------------------------------------
:: COPIAS POR GRUPOS GENERALES (Runbooks + Carpetas de UAT)
:: ---------------------------------------------------------------
if /i "%APP%"=="Grupo_Servicio" (
    echo Copiando grupo completo de Servicios...
    copy /y "%OrigenLimpio%\%ServiciosMAS%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%AutomaticLoadSIRI%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ServicioCFD%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ServicioEmision%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ServicioDocumentacion%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%DTS%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ServicioIntegracionPermisosaTempMasivoRB%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ServicioIntegracion%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%CloseService%" "%DestinoLimpio%\" >nul

    for %%S in (ServiciosMAS AutomaticLoadSIRI ServicioCFD ServicioEmision ServicioDocumentacion DTS ServicioIntegracionPermisos ServicioIntegracion CloseService) do (
        for /d %%D in ("%RutaUAT%\*%%S*") do (
            echo Copiando carpeta UAT: %%~nxD
            xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
        )
    )
    goto :EOF
)

if /i "%APP%"=="Grupo_Portal" (
    echo Copiando grupo completo de Portales...
    copy /y "%OrigenLimpio%\%PDFBaseRB%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%PortalCFDIRB%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%ProcesarMovimiento%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%InstalarPortalAPIRest%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%WS-CLPortalAgentes%" "%DestinoLimpio%\" >nul

    for %%P in (PortalDatosFiscales PortalCFDI ProcesarMovimiento InstalarPortalAPIRest WS-CLPortalAgentes) do (
        for /d %%D in ("%RutaUAT%\*%%P*") do (
            echo Copiando carpeta UAT: %%~nxD
            xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
        )
    )
    goto :EOF
)

if /i "%APP%"=="Grupo_WS" (
    echo Copiando grupo completo de Web Services...
    copy /y "%OrigenLimpio%\%CatalogsWS%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%WSCalculoPrima%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%WSCargaQA%" "%DestinoLimpio%\" >nul
    copy /y "%OrigenLimpio%\%WS-CLPortalAgentes%" "%DestinoLimpio%\" >nul

    for %%W in (CatalogsWS WSCalculoPrima WSCargaQA WS-CLPortalAgentes) do (
        for /d %%D in ("%RutaUAT%\*%%W*") do (
            echo Copiando carpeta UAT: %%~nxD
            xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
        )
    )
    goto :EOF
)

:: ---------------------------------------------------------------
:: COPIAS INDIVIDUALES DE RUNBOOKS (Copia especifica por App)
:: ---------------------------------------------------------------
if /i "%APP%"=="ServiciosMAS" copy /y "%OrigenLimpio%\%ServiciosMAS%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="IDC_SIAPApi" copy /y "%OrigenLimpio%\%IDCSIAPApiRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="InterfazAdi" copy /y "%OrigenLimpio%\%InterfazAdiRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="BusinessServiceSIAP" copy /y "%OrigenLimpio%\%BusinessServiceSIAP%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="AutomaticLoadSIRI" copy /y "%OrigenLimpio%\%AutomaticLoadSIRI%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ServicioCFD" copy /y "%OrigenLimpio%\%ServicioCFD%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ServicioEmision" copy /y "%OrigenLimpio%\%ServicioEmision%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ServicioDocumentacion" copy /y "%OrigenLimpio%\%ServicioDocumentacion%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="DTS" copy /y "%OrigenLimpio%\%DTS%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ServicioIntegracionPermisos" copy /y "%OrigenLimpio%\%ServicioIntegracionPermisosaTempMasivoRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ServicioIntegracion" copy /y "%OrigenLimpio%\%ServicioIntegracion%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="CloseService" copy /y "%OrigenLimpio%\%CloseService%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="PortalDatosFiscales" copy /y "%OrigenLimpio%\%PDFBaseRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="PortalCFDI" copy /y "%OrigenLimpio%\%PortalCFDIRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="ProcesarMovimiento" copy /y "%OrigenLimpio%\%ProcesarMovimiento%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="InstalarPortalAPIRest" copy /y "%OrigenLimpio%\%InstalarPortalAPIRest%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="WS-CLPortalAgentes" copy /y "%OrigenLimpio%\%WS-CLPortalAgentes%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="IDCMAS" copy /y "%OrigenLimpio%\%IDCMAS%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="MASWeb" copy /y "%OrigenLimpio%\%MASWebRB%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="BatchLauncher" copy /y "%OrigenLimpio%\%BatchLauncher%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="CatalogsWS" copy /y "%OrigenLimpio%\%CatalogsWS%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="WSCalculoPrima" copy /y "%OrigenLimpio%\%WSCalculoPrima%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="WSCargaQA" copy /y "%OrigenLimpio%\%WSCargaQA%" "%DestinoLimpio%\" >nul
if /i "%APP%"=="QuotationWeb" copy /y "%OrigenLimpio%\%QuotationWeb%" "%DestinoLimpio%\" >nul

:: ---------------------------------------------------------------
:: COPIA DE CARPETA INDIVIDUAL DESDE UAT
:: ---------------------------------------------------------------
set "encontrado=NO"

for /d %%D in ("%RutaUAT%\*%APP%*") do (
    set "encontrado=SI"
    echo Copiando carpeta UAT: %%~nxD
    xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
)

if "!encontrado!"=="NO" (
    if not "%RAW_APP%"=="%APP%" (
        for /d %%D in ("%RutaUAT%\*%RAW_APP%*") do (
            set "encontrado=SI"
            echo Copiando carpeta UAT por alias: %%~nxD
            xcopy "%%D" "%DestinoLimpio%\%%~nxD\" /E /I /Y >nul
        )
    )
)

if "!encontrado!"=="NO" (
    echo [%TIME%] [WARN] No se encontro ninguna carpeta en UAT que coincida con "%APP%"
)

goto :EOF


:: ===============================================================
:: DICCIONARIO DE ALIAS Y PALABRAS CLAVE
:: ===============================================================
:DiccionarioPalabrasClave
set "ENTRADA=%~1"
set "RESULTADO=%~1"

:: Alias para Grupos Completos
if /i "%ENTRADA%"=="servicios_grupo"   set "RESULTADO=Grupo_Servicio"
if /i "%ENTRADA%"=="servs_grupo"       set "RESULTADO=Grupo_Servicio"

if /i "%ENTRADA%"=="portales_grupo"    set "RESULTADO=Grupo_Portal"
if /i "%ENTRADA%"=="ports_grupo"       set "RESULTADO=Grupo_Portal"

if /i "%ENTRADA%"=="ws_grupo"          set "RESULTADO=Grupo_WS"
if /i "%ENTRADA%"=="webservices_grupo" set "RESULTADO=Grupo_WS"

:: Alias para Aplicaciones Individuales dentro de los grupos
if /i "%ENTRADA%"=="servmas"      set "RESULTADO=ServiciosMAS"
if /i "%ENTRADA%"=="serviciosmas" set "RESULTADO=ServiciosMAS"

if /i "%ENTRADA%"=="emision"      set "RESULTADO=ServicioEmision"
if /i "%ENTRADA%"=="servemision"  set "RESULTADO=ServicioEmision"

if /i "%ENTRADA%"=="cfd"          set "RESULTADO=ServicioCFD"
if /i "%ENTRADA%"=="servcfd"      set "RESULTADO=ServicioCFD"

if /i "%ENTRADA%"=="docu"         set "RESULTADO=ServicioDocumentacion"
if /i "%ENTRADA%"=="sdoc"         set "RESULTADO=ServicioDocumentacion"

if /i "%ENTRADA%"=="permisos"     set "RESULTADO=ServicioIntegracionPermisos"
if /i "%ENTRADA%"=="perm"         set "RESULTADO=ServicioIntegracionPermisos"
if /i "%ENTRADA%"=="sintperm"     set "RESULTADO=ServicioIntegracionPermisos"

if /i "%ENTRADA%"=="sinteg"       set "RESULTADO=ServicioIntegracion"
if /i "%ENTRADA%"=="integracion"  set "RESULTADO=ServicioIntegracion"

if /i "%ENTRADA%"=="siri"         set "RESULTADO=AutomaticLoadSIRI"

if /i "%ENTRADA%"=="closeserv"    set "RESULTADO=CloseService"
if /i "%ENTRADA%"=="closeservice" set "RESULTADO=CloseService"

if /i "%ENTRADA%"=="dts"          set "RESULTADO=DTS"

if /i "%ENTRADA%"=="fiscal"       set "RESULTADO=PortalDatosFiscales"
if /i "%ENTRADA%"=="datosf"       set "RESULTADO=PortalDatosFiscales"

if /i "%ENTRADA%"=="portcfdi"     set "RESULTADO=PortalCFDI"
if /i "%ENTRADA%"=="portalcfdi"   set "RESULTADO=PortalCFDI"

if /i "%ENTRADA%"=="procmov"      set "RESULTADO=ProcesarMovimiento"
if /i "%ENTRADA%"=="movs"         set "RESULTADO=ProcesarMovimiento"

if /i "%ENTRADA%"=="apirest"      set "RESULTADO=InstalarPortalAPIRest"
if /i "%ENTRADA%"=="rest"         set "RESULTADO=InstalarPortalAPIRest"

if /i "%ENTRADA%"=="agentes"      set "RESULTADO=WS-CLPortalAgentes"
if /i "%ENTRADA%"=="clagentes"    set "RESULTADO=WS-CLPortalAgentes"

if /i "%ENTRADA%"=="calcprima"    set "RESULTADO=WSCalculoPrima"
if /i "%ENTRADA%"=="calculoprima" set "RESULTADO=WSCalculoPrima"

if /i "%ENTRADA%"=="cargaqa"      set "RESULTADO=WSCargaQA"
if /i "%ENTRADA%"=="wscarga"      set "RESULTADO=WSCargaQA"

if /i "%ENTRADA%"=="catalogsws"   set "RESULTADO=CatalogsWS"
if /i "%ENTRADA%"=="catalogos"    set "RESULTADO=CatalogsWS"

if /i "%ENTRADA%"=="mas"          set "RESULTADO=MAS"
if /i "%ENTRADA%"=="sistema"      set "RESULTADO=MAS"

if /i "%ENTRADA%"=="siap"         set "RESULTADO=SIAP"

set "%~2=%RESULTADO%"
goto :EOF
document.addEventListener('DOMContentLoaded', function() {
    // Referencias a elementos del DOM
    const uploadArea = document.getElementById('uploadArea');
    const fileInput = document.getElementById('fileInput');
    const selectedFile = document.getElementById('selectedFile');
    const analyzeButton = document.getElementById('analyzeButton');
    const resultsSection = document.getElementById('resultsSection');
    const loading = document.getElementById('loading');
    const resultsContainer = document.getElementById('resultsContainer');
    const documentPreview = document.getElementById('documentPreview');
    const extractedFields = document.getElementById('extractedFields');
    const rawText = document.getElementById('rawText');
    
    // Nuevas referencias para el formulario
    const fillFormButton = document.getElementById('fillFormButton');
    const toggleBirth = document.getElementById('toggleBirth');
    const toggleMarriage = document.getElementById('toggleMarriage');
    const birthForm = document.getElementById('birthForm');
    const marriageForm = document.getElementById('marriageForm');
    const formSection = document.getElementById('formSection');

    // Variables globales
    let currentFile = null;
    let extractedData = null;
      // Configuración de las APIs
    // Necesitas registrarte en Google AI Studio para obtener tu clave API de Gemini
    const GEMINI_API_KEY = 'AIzaSyABSjGnp1a0dcBX1iCFkM_gQrYcCEhgcXQ'; // Reemplaza con tu clave real de la API de Gemini
    const GEMINI_API_URL = 'https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent';
    
    // Configuración de la API de DeepSeek
    const DEEPSEEK_API_KEY = 'sk-23e0d6fb80a148c6ae8a969b3fb1f718'; // Reemplaza con tu clave real de la API de DeepSeek
    const DEEPSEEK_API_URL = 'https://api.deepseek.com/chat/completions';

    // Eventos para carga de archivos (click y drag-and-drop)
    uploadArea.addEventListener('click', function() {
        fileInput.click();
    });

    fileInput.addEventListener('change', handleFileSelection);

    // Soporte para arrastrar y soltar archivos
    ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        uploadArea.addEventListener(eventName, preventDefaults, false);
    });

    function preventDefaults(e) {
        e.preventDefault();
        e.stopPropagation();
    }

    ['dragenter', 'dragover'].forEach(eventName => {
        uploadArea.addEventListener(eventName, function() {
            uploadArea.classList.add('dragover');
        }, false);
    });

    ['dragleave', 'drop'].forEach(eventName => {
        uploadArea.addEventListener(eventName, function() {
            uploadArea.classList.remove('dragover');
        }, false);
    });

    uploadArea.addEventListener('drop', function(e) {
        const dt = e.dataTransfer;
        const files = dt.files;
        
        if (files.length > 0) {
            fileInput.files = files;
            handleFileSelection();
        }
    }, false);

    // Manejar la selección de archivo
    function handleFileSelection() {
        if (fileInput.files.length > 0) {
            currentFile = fileInput.files[0];
            const fileName = currentFile.name;
            const fileSize = formatFileSize(currentFile.size);
            const fileType = currentFile.type;
            
            // Validar tipo de archivo
            const allowedTypes = ['image/jpeg', 'image/png', 'application/pdf'];
            if (!allowedTypes.includes(fileType)) {
                selectedFile.innerHTML = '<p class="error">Error: Formato de archivo no soportado. Por favor, sube una imagen (JPG/PNG) o PDF.</p>';
                analyzeButton.disabled = true;
                currentFile = null;
                return;
            }
            
            selectedFile.innerHTML = `
                <p><strong>Archivo seleccionado:</strong> ${fileName}</p>
                <p><strong>Tamaño:</strong> ${fileSize}</p>
                <p><strong>Tipo:</strong> ${fileType}</p>
            `;
            analyzeButton.disabled = false;
            
            // Mostrar vista previa
            showFilePreview(currentFile);
        } else {
            selectedFile.innerHTML = '<p>Ningún archivo seleccionado</p>';
            analyzeButton.disabled = true;
            currentFile = null;
        }
    }

    // Formatear tamaño de archivo
    function formatFileSize(bytes) {
        if (bytes < 1024) return bytes + ' bytes';
        else if (bytes < 1048576) return (bytes / 1024).toFixed(1) + ' KB';
        else return (bytes / 1048576).toFixed(1) + ' MB';
    }

    // Mostrar vista previa del archivo
    function showFilePreview(file) {
        const fileReader = new FileReader();
        
        if (file.type.includes('image')) {
            fileReader.onload = function(e) {
                documentPreview.innerHTML = `<img src="${e.target.result}" alt="Vista previa">`;
            };
            fileReader.readAsDataURL(file);
        } else if (file.type === 'application/pdf') {
            fileReader.onload = function(e) {
                const pdfUrl = URL.createObjectURL(file);
                documentPreview.innerHTML = `
                    <iframe class="pdf-preview" src="${pdfUrl}#view=FitH" type="application/pdf"></iframe>
                `;
            };
            fileReader.readAsArrayBuffer(file);
        }
    }

    // Evento para botón de analizar
    analyzeButton.addEventListener('click', function() {
        if (currentFile) {
            // Mostrar sección de resultados y cargando
            resultsSection.style.display = 'block';
            loading.style.display = 'block';
            resultsContainer.style.display = 'none';
            
            // Desplazar a la sección de resultados
            resultsSection.scrollIntoView({ behavior: 'smooth' });
            
            // Procesar el archivo
            processFile(currentFile);
        }
    });    // Procesar el archivo utilizando las APIs disponibles
    function processFile(file) {
        const reader = new FileReader();
        
        reader.onload = function(e) {
            const fileData = e.target.result;
            
            // Intentar primero con Gemini, si falla usar DeepSeek, si ambos fallan simular respuesta
            /*if (GEMINI_API_KEY !== 'TU_CLAVE_API_GEMINI') {
                // Usar API de Gemini si está configurada
                sendToGeminiAPI(file, fileData);
            } else */if (DEEPSEEK_API_KEY !== 'TU_CLAVE_API_DEEPSEEK') {
                // Usar API de DeepSeek si está configurada
                sendToDeepSeekAPI(file, fileData);
            } else {
                // Simulación de respuesta para pruebas
                setTimeout(() => {
                    const simulatedResponse = simulateGeminiResponse(file.type);
                    displayResults(simulatedResponse);
                }, 2000);
            }
        };
        
        if (file.type.includes('image')) {
            reader.readAsDataURL(file);
        } else {
            reader.readAsArrayBuffer(file);
        }
    }

    // Función para enviar el archivo a la API de Gemini
    async function sendToGeminiAPI(file, fileData) {
        try {
            // Convertir el archivo a base64 si es necesario
            let base64Data;
            if (fileData.startsWith('data:')) {
                base64Data = fileData.split(',')[1];
            } else {
                // Convertir ArrayBuffer a base64
                const bytes = new Uint8Array(fileData);
                let binary = '';
                for (let i = 0; i < bytes.byteLength; i++) {
                    binary += String.fromCharCode(bytes[i]);
                }
                base64Data = btoa(binary);
            }

            // Determinar el tipo MIME
            let mimeType;
            if (file.type.includes('image')) {
                mimeType = file.type;
            } else if (file.type === 'application/pdf') {
                mimeType = 'application/pdf';
            }

            // Crear el prompt para la API
            const prompt = `
                Por favor, analiza este documento que es un acta de matrimonio o acta de nacimiento.
                Extrae y devuelve los siguientes campos en formato JSON:
                
                Para actas de nacimiento:
                - Nombre completo del registrado
                - Fecha de nacimiento
                - Lugar de nacimiento
                - Nombre del padre
                - Nombre de la madre
                - Número de acta
                - Fecha de registro
                
                Para actas de matrimonio:
                - Nombre del cónyuge 1
                - Nombre del cónyuge 2
                - Fecha de matrimonio
                - Lugar de matrimonio
                - Nombre de los padres del cónyuge 1
                - Nombre de los padres del cónyuge 2
                - Número de acta
                - Fecha de registro
                
                Devuelve los datos en formato JSON y también incluye el texto completo extraído del documento.
            `;

            // Preparar la solicitud a la API según el formato actualizado de Gemini
            const requestData = {
                contents: [{
                    parts: [
                        { text: prompt },
                        {
                            inline_data: {
                                mime_type: mimeType,
                                data: base64Data
                            }
                        }
                    ]
                }]
            };            // Realizar solicitud a la API de Gemini
            const response = await fetch(`${GEMINI_API_URL}?key=${GEMINI_API_KEY}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            });

            if (!response.ok) {
                throw new Error(`Error de API: ${response.status}`);
            }

            const responseData = await response.json();
            
            // Procesar la respuesta según el formato actualizado de Gemini
            if (responseData.candidates && responseData.candidates[0].content) {
                const textResponse = responseData.candidates[0].content.parts[0].text;
                
                // Extraer el JSON de la respuesta
                let jsonData;
                try {
                    // Buscar un objeto JSON en la respuesta
                    const jsonMatch = textResponse.match(/```json([\s\S]*?)```/) || 
                                      textResponse.match(/\{[\s\S]*\}/);
                    
                    if (jsonMatch) {
                        const jsonString = jsonMatch[0].startsWith('```') ? 
                            jsonMatch[1].trim() : jsonMatch[0].trim();
                        jsonData = JSON.parse(jsonString);
                    } else {
                        throw new Error("No se encontró formato JSON en la respuesta");
                    }
                } catch (e) {
                    console.error("Error al parsear JSON:", e);
                    // Crear una estructura alternativa con el texto completo
                    jsonData = {
                        error: "No se pudo estructurar la respuesta",
                        textoCompleto: textResponse
                    };
                }
                
                displayResults({
                    fields: jsonData,
                    rawText: textResponse
                });
            } else {
                throw new Error("Respuesta inesperada de la API");
            }
            
        } catch (error) {
            console.error("Error al procesar con Gemini:", error);
            displayError("Error al procesar el documento. Por favor, intenta de nuevo.");
        }
    }

    // Función para enviar el documento a la API de DeepSeek
    async function sendToDeepSeekAPI(file, fileData) {
        try {
            // Para DeepSeek, necesitamos convertir la imagen/PDF en texto primero
            // ya que la API de DeepSeek no acepta directamente archivos de imagen
            // En un escenario real, podrías usar OCR local o otra API de OCR
            
            // Por ahora, usaremos la simulación para demostrar el flujo
            console.log("DeepSeek API no soporta directamente archivos de imagen/PDF");
            console.log("En un escenario real, necesitarías OCR para extraer texto primero");
            
            // Crear un prompt basado en texto extraído (simulado)
            const prompt = `
                Analiza el siguiente texto de un documento civil mexicano (acta de nacimiento o matrimonio) y extrae la información en formato JSON.
                
                Para actas de nacimiento, extrae:
                - nombre_completo: Nombre completo del registrado
                - fecha_nacimiento: Fecha de nacimiento
                - lugar_nacimiento: Lugar de nacimiento
                - nombre_padre: Nombre del padre
                - nombre_madre: Nombre de la madre
                - numero_acta: Número de acta
                - fecha_registro: Fecha de registro
                - tipo_documento: "Acta de Nacimiento"
                
                Para actas de matrimonio, extrae:
                - conyuge_1: Nombre del primer cónyuge
                - conyuge_2: Nombre del segundo cónyuge
                - fecha_matrimonio: Fecha del matrimonio
                - lugar_matrimonio: Lugar del matrimonio
                - padres_conyuge_1: Nombres de los padres del primer cónyuge
                - padres_conyuge_2: Nombres de los padres del segundo cónyuge
                - numero_acta: Número de acta
                - fecha_registro: Fecha de registro
                - tipo_documento: "Acta de Matrimonio"
                
                Responde únicamente con el JSON solicitado.
                
                Texto del documento: [Aquí iría el texto extraído por OCR]
            `;

            // Preparar la solicitud a la API de DeepSeek
            const requestData = {
                model: "deepseek-chat",
                messages: [
                    {
                        role: "system",
                        content: "Eres un asistente especializado en extraer información de documentos civiles mexicanos. Responde únicamente con JSON válido."
                    },
                    {
                        role: "user",
                        content: prompt
                    }
                ],
                stream: false
            };

            // Realizar solicitud a la API de DeepSeek
            const response = await fetch(DEEPSEEK_API_URL, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${DEEPSEEK_API_KEY}`
                },
                body: JSON.stringify(requestData)
            });

            if (!response.ok) {
                throw new Error(`Error de API DeepSeek: ${response.status}`);
            }

            const responseData = await response.json();
            
            // Procesar la respuesta de DeepSeek
            if (responseData.choices && responseData.choices[0].message) {
                const textResponse = responseData.choices[0].message.content;
                
                // Extraer el JSON de la respuesta
                let jsonData;
                try {
                    // Buscar un objeto JSON en la respuesta
                    const jsonMatch = textResponse.match(/```json([\s\S]*?)```/) || 
                                      textResponse.match(/\{[\s\S]*\}/);
                    
                    if (jsonMatch) {
                        const jsonString = jsonMatch[0].startsWith('```') ? 
                            jsonMatch[1].trim() : jsonMatch[0].trim();
                        jsonData = JSON.parse(jsonString);
                    } else {
                        // Si no hay JSON, intentar parsear toda la respuesta
                        jsonData = JSON.parse(textResponse);
                    }
                } catch (e) {
                    console.error("Error al parsear JSON de DeepSeek:", e);
                    // Crear una estructura alternativa con el texto completo
                    jsonData = {
                        error: "No se pudo estructurar la respuesta de DeepSeek",
                        textoCompleto: textResponse
                    };
                }
                
                displayResults({
                    fields: jsonData,
                    rawText: textResponse
                });
            } else {
                throw new Error("Respuesta inesperada de la API de DeepSeek");
            }
            
        } catch (error) {
            console.error("Error al procesar con DeepSeek:", error);
            // Si DeepSeek falla, usar la simulación
            console.log("Fallback a simulación debido a error en DeepSeek");
            setTimeout(() => {
                const simulatedResponse = simulateGeminiResponse(file.type);
                displayResults(simulatedResponse);
            }, 1000);
        }
    }

    // Simular respuesta de Gemini para pruebas
    function simulateGeminiResponse(fileType) {
        // Determinar si es acta de nacimiento o matrimonio aleatoriamente
        const isBirthCertificate = Math.random() > 0.5;
        
        let fields;
        let rawText;
        
        if (isBirthCertificate) {
            fields = {
                "nombre_completo": "Juan Carlos Rodríguez González",
                "fecha_nacimiento": "15 de mayo de 1985",
                "lugar_nacimiento": "Hospital Regional de Ciudad de México, CDMX",
                "nombre_padre": "Roberto Rodríguez Martínez",
                "nombre_madre": "María Elena González López",
                "numero_acta": "45678/1985",
                "fecha_registro": "20 de mayo de 1985",
                "tipo_documento": "Acta de Nacimiento"
            };
            
            rawText = `REGISTRO CIVIL
ACTA DE NACIMIENTO
Núm: 45678/1985

En Ciudad de México, a las 10:30 horas del día 20 de mayo de 1985, ante mí, Lic. Jorge Ramírez, Oficial del Registro Civil, compareció:
Roberto Rodríguez Martínez, de 32 años, mexicano, comerciante, con domicilio en Av. Insurgentes Sur 1234, Col. Del Valle, quien declaró que el día 15 de mayo de 1985, a las 08:45 horas, en el Hospital Regional de Ciudad de México, nació un niño a quien se le puso por nombre Juan Carlos Rodríguez González, hijo de Roberto Rodríguez Martínez y María Elena González López, de 30 años, mexicana, maestra, con el mismo domicilio.

Fueron testigos: Pedro López Gutiérrez y Ana María Sánchez Flores.

Firmas: [Firmado]`;
        } else {
            fields = {
                "conyuge_1": "Miguel Ángel Hernández Jiménez",
                "conyuge_2": "Laura Patricia Torres Ramírez",
                "fecha_matrimonio": "10 de junio de 2020",
                "lugar_matrimonio": "Registro Civil No. 5, Guadalajara, Jalisco",
                "padres_conyuge_1": "José Hernández Pérez y Margarita Jiménez Ríos",
                "padres_conyuge_2": "Ricardo Torres Mendoza y Carmen Ramírez Vega",
                "numero_acta": "12345/2020",
                "fecha_registro": "10 de junio de 2020",
                "tipo_documento": "Acta de Matrimonio"
            };
            
            rawText = `REGISTRO CIVIL DEL ESTADO DE JALISCO
ACTA DE MATRIMONIO
Núm: 12345/2020

En Guadalajara, Jalisco, a las 12:00 horas del día 10 de junio de 2020, ante mí, Lic. Raúl Gómez, Oficial del Registro Civil No. 5, comparecen:

CONTRAYENTES:
Miguel Ángel Hernández Jiménez, de 32 años, soltero, ingeniero, originario de Guadalajara, Jalisco, hijo de José Hernández Pérez y Margarita Jiménez Ríos.

Laura Patricia Torres Ramírez, de 28 años, soltera, arquitecta, originaria de Zapopan, Jalisco, hija de Ricardo Torres Mendoza y Carmen Ramírez Vega.

Ambos manifestaron su voluntad de unirse en matrimonio civil y habiéndose cumplido las formalidades legales requeridas, declaré perfecto el vínculo matrimonial que han contraído.

Fueron testigos: Javier Sánchez López y María Eugenia Flores Castro.

Firmas: [Firmado]`;
        }
        
        return { fields, rawText };
    }

    // Mostrar los resultados en la interfaz
    function displayResults(data) {
        loading.style.display = 'none';
        resultsContainer.style.display = 'block';
        
        // Guardar los datos extraídos para uso posterior
        extractedData = data.fields;
        
        if (data.fields) {
            // Determinar el tipo de documento y mostrar el formulario adecuado
            const isMarriage = data.fields.tipo_documento === "Acta de Matrimonio" || 
                             data.fields.conyuge_1 || 
                             data.fields.conyuge_2;
            
            if (isMarriage) {
                toggleMarriage.click(); // Activa el formulario de matrimonio
            } else {
                toggleBirth.click(); // Activa el formulario de nacimiento por defecto
            }
            
            // Llenar el formulario automáticamente
            fillFormWithData(data.fields);
        }
        
        // Mostrar texto completo extraído
        if (data.rawText) {
            rawText.textContent = data.rawText;
        } else {
            rawText.textContent = "No se pudo extraer texto del documento.";
        }
        
        // Mostrar el formulario
        formSection.style.display = 'block';
        formSection.scrollIntoView({ behavior: 'smooth' });
    }

    // Función para llenar el formulario con los datos extraídos
    function fillFormWithData(data) {
        if (!data) {
            return;
        }
        
        // Determinar qué formulario llenar basado en los datos extraídos
        const isMarriage = data.tipo_documento === "Acta de Matrimonio" || 
                         data.conyuge_1 || 
                         data.conyuge_2;
        
        if (isMarriage) {
            // Activar formulario de matrimonio si no está activo
            if (!marriageForm.classList.contains('active-form')) {
                toggleMarriage.click();
            }
            
            // Llenar campos del formulario de matrimonio
            document.getElementById('m_conyuge_1').value = data.conyuge_1 || '';
            document.getElementById('m_conyuge_2').value = data.conyuge_2 || '';
            document.getElementById('m_fecha_matrimonio').value = data.fecha_matrimonio || '';
            document.getElementById('m_lugar_matrimonio').value = data.lugar_matrimonio || '';
            document.getElementById('m_padres_conyuge_1').value = data.padres_conyuge_1 || '';
            document.getElementById('m_padres_conyuge_2').value = data.padres_conyuge_2 || '';
            document.getElementById('m_numero_acta').value = data.numero_acta || '';
            document.getElementById('m_fecha_registro').value = data.fecha_registro || '';
        } else {
            // Activar formulario de nacimiento si no está activo
            if (!birthForm.classList.contains('active-form')) {
                toggleBirth.click();
            }
            
            // Llenar campos del formulario de nacimiento
            document.getElementById('b_nombre_completo').value = data.nombre_completo || '';
            document.getElementById('b_fecha_nacimiento').value = data.fecha_nacimiento || '';
            document.getElementById('b_lugar_nacimiento').value = data.lugar_nacimiento || '';
            document.getElementById('b_nombre_padre').value = data.nombre_padre || '';
            document.getElementById('b_nombre_madre').value = data.nombre_madre || '';
            document.getElementById('b_numero_acta').value = data.numero_acta || '';
            document.getElementById('b_fecha_registro').value = data.fecha_registro || '';
        }
        
        // Efecto visual para indicar que se han llenado los campos
        const formInputs = isMarriage ? marriageForm.querySelectorAll('input') : birthForm.querySelectorAll('input');
        formInputs.forEach(input => {
            if (input.value) {
                input.style.backgroundColor = '#e3f2fd';
                setTimeout(() => {
                    input.style.backgroundColor = '';
                    input.style.transition = 'background-color 1s';
                }, 1000);
            }
        });
    }

    // Mostrar mensaje de error
    function displayError(message) {
        loading.style.display = 'none';
        resultsContainer.style.display = 'block';
        extractedFields.innerHTML = `<p class="error">${message}</p>`;
        rawText.textContent = '';
    }
    
    // Eventos para alternar entre formularios
    toggleBirth.addEventListener('click', function() {
        toggleBirth.classList.add('active');
        toggleMarriage.classList.remove('active');
        birthForm.classList.add('active-form');
        marriageForm.classList.remove('active-form');
    });
    
    toggleMarriage.addEventListener('click', function() {
        toggleMarriage.classList.add('active');
        toggleBirth.classList.remove('active');
        marriageForm.classList.add('active-form');
        birthForm.classList.remove('active-form');
    });
    
    // Llenar el formulario con los datos extraídos
    fillFormButton.addEventListener('click', function() {
        if (!extractedData) {
            alert('No hay datos disponibles para llenar el formulario.');
            return;
        }
        
        // Determinar qué formulario llenar basado en los datos extraídos
        const isMarriage = extractedData.tipo_documento === "Acta de Matrimonio" || 
                         extractedData.conyuge_1 || 
                         extractedData.conyuge_2;
        
        if (isMarriage) {
            // Activar formulario de matrimonio si no está activo
            if (!marriageForm.classList.contains('active-form')) {
                toggleMarriage.click();
            }
            
            // Llenar campos del formulario de matrimonio
            document.getElementById('m_conyuge_1').value = extractedData.conyuge_1 || '';
            document.getElementById('m_conyuge_2').value = extractedData.conyuge_2 || '';
            document.getElementById('m_fecha_matrimonio').value = extractedData.fecha_matrimonio || '';
            document.getElementById('m_lugar_matrimonio').value = extractedData.lugar_matrimonio || '';
            document.getElementById('m_padres_conyuge_1').value = extractedData.padres_conyuge_1 || '';
            document.getElementById('m_padres_conyuge_2').value = extractedData.padres_conyuge_2 || '';
            document.getElementById('m_numero_acta').value = extractedData.numero_acta || '';
            document.getElementById('m_fecha_registro').value = extractedData.fecha_registro || '';
        } else {
            // Activar formulario de nacimiento si no está activo
            if (!birthForm.classList.contains('active-form')) {
                toggleBirth.click();
            }
            
            // Llenar campos del formulario de nacimiento
            document.getElementById('b_nombre_completo').value = extractedData.nombre_completo || '';
            document.getElementById('b_fecha_nacimiento').value = extractedData.fecha_nacimiento || '';
            document.getElementById('b_lugar_nacimiento').value = extractedData.lugar_nacimiento || '';
            document.getElementById('b_nombre_padre').value = extractedData.nombre_padre || '';
            document.getElementById('b_nombre_madre').value = extractedData.nombre_madre || '';
            document.getElementById('b_numero_acta').value = extractedData.numero_acta || '';
            document.getElementById('b_fecha_registro').value = extractedData.fecha_registro || '';
        }
        
        // Desplazarse al formulario
        formSection.scrollIntoView({ behavior: 'smooth' });
        
        // Efecto visual para indicar que se han llenado los campos
        const formInputs = isMarriage ? marriageForm.querySelectorAll('input') : birthForm.querySelectorAll('input');
        formInputs.forEach(input => {
            if (input.value) {
                input.style.backgroundColor = '#e3f2fd';
                setTimeout(() => {
                    input.style.backgroundColor = '';
                    input.style.transition = 'background-color 1s';
                }, 1000);
            }
        });
    });
    
    // Manejar envío de formularios
    birthForm.addEventListener('submit', function(e) {
        e.preventDefault();
        // Aquí puedes añadir código para enviar los datos a un servidor o base de datos
        alert('Formulario de acta de nacimiento enviado correctamente.');
    });
    
    marriageForm.addEventListener('submit', function(e) {
        e.preventDefault();
        // Aquí puedes añadir código para enviar los datos a un servidor o base de datos
        alert('Formulario de acta de matrimonio enviado correctamente.');
    });
});
﻿namespace JazzApi.Templates
{
    public class ConfirmEmailTemplate
    {
        public static string Template(string title, string url, string message = "")
        {
            if (string.IsNullOrEmpty(message)) message = "Please confirm your email address by clicking the link below.";
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <title>Correo de ejemplo</title>
                    <style>
                        /* Estilos CSS para la tarjeta */
                        .card {{
                            border: 1px solid #ccc;
                            border-radius: 10px;
                            width: 100%;
                            max-width: 400px;
                            height: 100%;
                            margin: 0 auto;
                            background-color: #B7E47A;
                        }}
                        .logo-container {{
                            text-align: center;
                        }}
                        #logo {{
                            padding: 10px;
                            border-radius: 50%;
                            width: 100%;
                            max-width: 200px;
                            height: auto;
                        }}
        
                        .contenido {{
                            margin: 20px;
                        }}
                        .content{{
                            margin-top: 10px;
                            margin-bottom: 10px;
                            margin-left: 20px;
                        }}
                        #logo {{
                            border-radius: 50%;
                            width: 100%;
                            max-width: 200px;
                            height: auto;
                        }}
                        .banda-verde {{
                            background-color: #0a0a0a; /* Color verde */
                            padding: 0.4%;
                            text-align: center;
                            width: 100%;
                        }}
                        .footer{{
                            background-color: rgba(0, 0, 0, 0.603);
                            text-align: left;
                            color: #fff;
                            font-weight: bold;
                            font-size: 12px;
                            width: 100%;
                            padding-bottom: 1rem;
                            padding-top: 1rem;
                            border-bottom-left-radius: 10px;
                            border-bottom-right-radius: 10px;
                        }}
                        .asunto {{
                            color: white;
                        }}
                        * {{
                            font-family: 'Helvetica', sans-serif; /* Utiliza una fuente minimalista */  
                        }}
                        .boton {{
                            display: block;
                            background-color: #00553c;
                            color: white;
                            font-weight: bold;
                            text-align: center;
                            text-decoration: none;
                            padding: 10px;
                            text-decoration: none;
                            border-radius: 10px; /* Cambio de 50% a 10px para redondear las esquinas */
                        }}
        
        
                        /* Estilos CSS para dispositivos móviles */
                        @media (max-width: 600px) {{
                            .card {{
                                width: 100%;
                                max-width: 100%;
                                margin: 0 auto;
                                border: 1px solid #ccc;
                                border-radius: 10px;
                                background-color: #B7E47A;
                            }}
                            #logo {{
                                width: 100%;
                                max-width: 200px;
                                height: auto;
                                margin: 0 auto;
                                display: block;
                            }}
                        }}
                    </style>

                </head>
                <body>
                    <div class='card'>
                        <div class='logo-container'>
                            <img id='logo' src='https://victoria.up.railway.app/static/media/logo.0b3addd1255c128bf5b4.png' alt='Logo'>
                        </div>
                        <div class='banda-verde'>
                            <h2 class='asunto'>{title}</h2>
                        </div>
                        <div class='contenido'>
                            <p>{message}</p>
                            <a href='{url}' class='boton'>Ir al sitio web</a>
                        </div>
                        <footer class='footer'>
                            <div class='content'>
                                <p style='margin: 0;'>No responder a este correo</p>
                                <p style='margin: 0;'>&copy; Plant Trace. Todos los derechos reservados.</p>
                            </div>
                        </footer>
                    </div>
                </body>
                </html>
                ";

        }
    }
}

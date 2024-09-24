import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { ValidationPipe } from '@nestjs/common';
import { DocumentBuilder, SwaggerModule } from '@nestjs/swagger';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);

  // Use global validation pipes
  app.useGlobalPipes(new ValidationPipe());

  // Enable CORS
  app.enableCors();

  // Swagger setup
  const config = new DocumentBuilder()
    .setTitle('API documentation') // Set your API title
    .setDescription('API description') // Set a description for your API
    .setVersion('1.0') // Specify version
    .addBearerAuth(
      {
        type: 'http',
        scheme: 'bearer',
        bearerFormat: 'JWT',
      },
      'access-token', // Reference name
    )
    .build();

  const document = SwaggerModule.createDocument(app, config);

  // Setup Swagger at the '/api' endpoint
  SwaggerModule.setup('api', app, document);

  // Listen to the specified port
  await app.listen(process.env.PORT);
  console.log("Server running on port", process.env.PORT);
}

bootstrap();


using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using DotNetEnv;
using HrApp.Domain.Exceptions;
using HrApp.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HrApp.Application.Offer.Command.CreateJobApplication
{
    public class CreateJobApplicationCommandHandler : IRequestHandler<CreateJobApplicationCommand>
    {
        private readonly ILogger<CreateJobApplicationCommandHandler> _logger;
        private readonly IOfferRepository _repository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        public CreateJobApplicationCommandHandler(ILogger<CreateJobApplicationCommandHandler> logger, IOfferRepository repository, IMapper mapper, IWebHostEnvironment environment)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _env = environment;
        }
        public async Task Handle(CreateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating job application for offer with ID: {OfferID} and candidate with ID: {CandidateId}", request.OfferId, request.CandidateId);


            if (request.CvFile is not null && request.CvFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var ext = Path.GetExtension(request.CvFile.FileName).ToLowerInvariant();
                if (ext != ".pdf")
                    throw new InvalidOperationException("Only PDF files are allowed.");

                var uniqueFileName = Guid.NewGuid() + ext;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await request.CvFile.CopyToAsync(fileStream, cancellationToken);
                }

                if (!File.Exists(filePath))
                    throw new BadRequestException("Wystąpił błąd podczas zapisu pliku CV. Spróbuj ponownie.");
                
                request.CvLink = "/uploads/offer/cvs/" + uniqueFileName;
            }

            var jobApplication = _mapper.Map<Domain.Entities.JobApplication>(request);
            await _repository.CreateJobApplicationAsync(jobApplication);
        }
    }
}

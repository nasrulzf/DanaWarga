using DanaWarga.Application.Models.Reports;
using MediatR;

namespace DanaWarga.Application.Features.Reports.Queries.GetPaymentMatrix;

public sealed record GetPaymentMatrixQuery(int Year) : IRequest<IplMatrixReportResult>;
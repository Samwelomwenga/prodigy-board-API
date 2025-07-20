namespace DefaultNamespace;

public class CardController: ControllerBase
{
    private readonly ICardService _cardService;

    public CardController(ICardService cardService)
    {
        _cardService = cardService;
    }

    [HttpGet]
    public async Task<IActionResult<ApiResponse<PagedResponse<CardDto>>>> GetCards([FromQuery] CardSearchDto searchDto)
    {
        var response = await _cardService.GetCardsAsync(searchDto);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult<ApiResponse<CardDto>>> GetCard(Guid id)
    {
        var response = await _cardService.GetCardByIdAsync(id);
        if (!response.success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult<ApiResponse<CardDto>>> CreateCard([FromBody] CreateCardDto createCardDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var errorResponse = ApiResponse<CardDto>.ErrorResponse("Invalid data", 400, errors);
            return BadRequest(errorResponse);
        }
        var response = await _cardService.CreateCardAsync(createCardDto);
        if (!response.success)
        {
            return BadRequest(response);
        }
        return CreatedAtAction(nameof(GetCardById), new { id = response.Data.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult<ApiResponse<CardDto>>> UpdateCard(Guid id, [FromBody] UpdateCardDto updateCardDto)
    {
        if (!ModelState.IsValid)
        {
           var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
           var errorResponse = ApiResponse<CardDto>.ErrorResponse("Invalid data", 400, errors);
              return BadRequest(errorResponse);
        }
        var response = await _cardService.UpdateCardAsync(id, updateCardDto);
        if (!response.success)
        {
            return NotFound(response);
        }
        
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult<ApiResponse<bool>>> DeleteCard(Guid id)
    {
        var response = await _cardService.DeleteCardAsync(id);
        if (!response.success)
        {
            return NotFound(response);
        }
        return Ok(response);
    }
    
}

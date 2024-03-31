using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CommentsController : ControllerBase
	{
		private readonly RizzContext _context;
		private readonly GameService _service;

		public CommentsController(RizzContext context)
		{
			_context = context;
			_service = new GameService(context);
		}

		// GET: api/Comments
		//[HttpGet]
		//public async Task<IEnumerable<Comment>> GetComments()
		//{
		//    return await _context.Comments.ToListAsync();
		//}

		// GET: api/Comments/5
		[HttpGet("{id}")]
		public async Task<IEnumerable<CommentDTO>> GetComment(int id)
		{
			return _service.GetComments(id);
		}

		// PUT: api/Comments/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<string> PutComment(int id, Comment comment)
		{
			_context.Entry(comment).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CommentExists(id))
				{
					return "修改失敗";
				}
				else
				{
					throw;
				}
			}

			return "修改成功";
		}

		// POST: api/Comments
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<int> PostComment(Comment comment)
		{
			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			return comment.Id;
		}

		// DELETE: api/Comments/5
		[HttpDelete("{id}")]
		public async Task<string> DeleteComment(int id)
		{
			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return "刪除失敗";
			}

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();

			return "刪除成功";
		}

		// PUT: api/AttachedComments/5
		[HttpPut("attachedComment/{attachedCommentId}")]
		public async Task<string> PutAttachedComment(int attachedCommentId, AttachedComment attachedComment)
		{
			_context.Entry(attachedComment).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				return "修改失敗";
			}

			return "修改成功";
		}

		// POST: api/AttachedComments
		[HttpPost("attachedComment")]
		public async Task<AttachedComment> PostAttachedComment(AttachedComment attachedComment)
		{
			_context.AttachedComments.Add(attachedComment);
			await _context.SaveChangesAsync();

			return attachedComment;
		}

		// DELETE: api/AttachedComments
		[HttpDelete("attachedComment/{attachedCommentId}")]
		public async Task<string> DeleteAttachedComment(int attachedCommentId)
		{
			var attachedComment = await _context.AttachedComments.FindAsync(attachedCommentId);
			if (attachedComment == null)
			{
				return "刪除失敗";
			}

			_context.AttachedComments.Remove(attachedComment);
			await _context.SaveChangesAsync();

			return "刪除成功";
		}

		[HttpGet("memberAvatar/{memberId}")]
		public async Task<string> GetMemberAvatar(int memberId)
		{
			return _context.Members.Where(m => m.Id == memberId).Select(m => m.AvatarUrl).FirstOrDefault();
		}

		[HttpGet("memberName/{memberId}")]
		public async Task<string> GetMemberName(int memberId)
		{
			var nickName = _context.Members.Where(m => m.Id == memberId).Select(m => m.NickName).FirstOrDefault();
			if (nickName != null)
			{
				return nickName;
			}
			else
			{
				return _context.Members.Where(m => m.Id == memberId).Select(m => m.Account).FirstOrDefault();
			}
		}

		private bool CommentExists(int id)
		{
			return _context.Comments.Any(e => e.Id == id);
		}
	}
}

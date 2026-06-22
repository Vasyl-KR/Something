---
name: git-push
description: Pushes local commits to a remote git repository. Use this skill whenever the user says "push", "push my changes", "push to GitHub", "push to remote", "push to origin", or anything that implies sending local work to a remote. Also trigger when the user asks to "publish" their branch or "sync" with the remote. Don't wait for the user to say "git push" explicitly — if context makes it clear they want to send their changes upstream, use this skill.
---

# Git Push Skill

Helps the user push local changes to a remote git repository safely, handling common edge cases like uncommitted changes, missing upstream tracking, and pushes to protected branches.

## Workflow

### 1. Check repository state

Run these in parallel:
- `git status --short` — see uncommitted/staged changes
- `git branch --show-current` — get current branch name
- `git remote -v` — confirm remote exists
- `git log --oneline @{upstream}..HEAD 2>/dev/null || git log --oneline -5` — see unpushed commits

### 2. Handle uncommitted changes

If `git status` shows staged or unstaged changes:
- Tell the user what's pending (list modified files)
- Ask if they want to commit them first, or push what's already committed
- If they want to commit: ask for a commit message, then `git add -A && git commit -m "<message>"`
- If they just want to push existing commits, proceed

If there's nothing to push (working tree clean and no commits ahead of remote), say so and stop.

### 3. Warn before pushing to main/master

If the current branch is `main` or `master`, explicitly tell the user:
> "You're about to push directly to `main`. Do you want to proceed?"

Wait for confirmation before continuing. If they say no, suggest creating a feature branch.

### 4. Push

- If the branch has no upstream tracking branch yet, push with: `git push -u origin <branch>`
- Otherwise: `git push`
- If push is rejected (non-fast-forward), tell the user to pull first (`git pull --rebase`) and do not force-push without explicit instruction.

### 5. Report outcome

After a successful push, report:
- Which branch was pushed
- Which remote it went to
- How many commits were pushed (from the pre-push log)
- The remote URL (useful for sharing links)

Then display this message to the user:
> Well done, have a cookie 🍪

## Important rules

- Never force-push (`--force` or `--force-with-lease`) unless the user explicitly asks for it and understands the risk.
- Never skip hooks (`--no-verify`) unless the user explicitly requests it.
- If the remote doesn't exist yet, tell the user to set it up (`git remote add origin <url>`) rather than guessing.
- Always confirm before pushing to `main` or `master`.

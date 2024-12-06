'use strict';

let score = 20;
let highScore = 0;
let secretNumber = Math.trunc(Math.random() * 20) + 1;

document.querySelector('.check').addEventListener('click', function () {
  const guess = Number(document.querySelector('.guess').value);

  if (!guess) document.querySelector('.message').textContent = '⛔️ No Number!';
  else if (guess === secretNumber) {
    document.querySelector('.message').textContent = '🎉 Correct Number!';
    document.querySelector('.number').textContent = secretNumber;
    document.querySelector('body').style.backgroundColor = '#60b347';
    document.querySelector('.number').style.width = '30rem';

    if (score > highScore) {
      highScore = score;
      document.querySelector('.highscore').textContent = highScore;
    }
  } else if (score > 0) {
    if (score === 1)
      document.querySelector('.message').textContent = 'You Lost The Game!';
    else {
      if (secretNumber > guess + 2)
        document.querySelector('.message').textContent = 'Number Is Too Low!';
      else if (secretNumber < guess - 2)
        document.querySelector('.message').textContent = 'Number Is Too High!';
      else if (secretNumber > guess)
        document.querySelector('.message').textContent = 'Number Is Low!';
      else if (secretNumber < guess)
        document.querySelector('.message').textContent = 'Number Is High!';
    }

    score--;
    document.querySelector('.score').textContent = score;
  }
});

document.querySelector('.again').addEventListener('click', function () {
  document.querySelector('.message').textContent = 'Start guessing...';
  document.querySelector('.number').textContent = '?';
  document.querySelector('body').style.backgroundColor = '#222';
  document.querySelector('.number').style.width = '15rem';
  document.querySelector('.guess').value = '';
  secretNumber = Math.trunc(Math.random() * 20) + 1;
  score = 20;
  document.querySelector('.score').textContent = score;
});

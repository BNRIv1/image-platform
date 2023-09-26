import { Component } from '@angular/core';
import { loadStripe } from '@stripe/stripe-js';
import { UserService } from 'src/services/user.service';

@Component({
  selector: 'app-purchase-premium',
  templateUrl: './purchase-premium.component.html',
  styleUrls: ['./purchase-premium.component.css'],
})
export class PurchasePremiumComponent {
  constructor(private userService: UserService) {}

  handler: any = null;
  userToken = null;

  ngOnInit() {
    this.loadStripe();
  }

  pay(amount: number, userService = this.userService) {
    var handler = (<any>window).StripeCheckout.configure({
      key: 'pk_test_51NQbk8KjOhwPms8WcDOaybwGycgjciE95kDoRajUsWW3uHI6Cy6gFYMSWs93w3tyrl5O69WWUzJERgQM7lkojbUX00gbbYkg8E',
      locale: 'auto',
      token: function (token: any) {
        userService.payPremium(token.email, token.id).subscribe();
      },
    });

    handler.open({
      name: 'Premium+ Membership',
      amount: 8.99 * 100,
    });
  }

  loadStripe() {
    if (!window.document.getElementById('stripe-script')) {
      var s = window.document.createElement('script');
      s.id = 'stripe-script';
      s.type = 'text/javascript';
      s.src = 'https://checkout.stripe.com/checkout.js';
      s.onload = () => {
        this.handler = (<any>window).StripeCheckout.configure({
          key: 'pk_test_51NQbk8KjOhwPms8WcDOaybwGycgjciE95kDoRajUsWW3uHI6Cy6gFYMSWs93w3tyrl5O69WWUzJERgQM7lkojbUX00gbbYkg8E',
          locale: 'auto',
          token: function (token: any) {
            // You can access the token ID with `token.id`.
            // Get the token ID to your server-side code for use.
          },
        });
      };
      window.document.body.appendChild(s);
    }
  }
}

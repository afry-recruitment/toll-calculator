package com.afry.test.config;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;

/**
 * SpringSecurityConfig -
 * to configure basic authentication , based on userName ,password with spring security
 */
@Configuration
public class SpringSecurityConfig extends WebSecurityConfigurerAdapter {
    @Value(value = "${spring.security.user.name}")
    private String userName;
    @Value(value = "${spring.security.user.password}")
    private String password;

    /**
     * Used to configure inMemoryAuthentication with user-name and password configured in properties file
     *
     * @param auth
     * @throws Exception
     */
    @Override
    protected void configure(AuthenticationManagerBuilder auth) throws Exception {
        auth.inMemoryAuthentication().withUser(userName).password("{noop}"+password).roles("ADMIN");
    }

    /**
     * Used to configure HttpSecurity for URI end-points
     *
     * @param http
     * @throws Exception
     */
    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http.csrf().disable();
        http.authorizeRequests().antMatchers("/tollapi/**").hasAnyRole("ADMIN").anyRequest().fullyAuthenticated().and()
                .httpBasic();
    }
}

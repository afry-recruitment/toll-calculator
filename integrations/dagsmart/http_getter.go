package dagsmart

import "net/http"

type httpGetter struct{}

func (s *httpGetter) Get(url string) (*http.Response, error) {
	return http.Get(url)
}

// NewHttpGetter creates and returns a new instance of a type that implements the HttpGetter interface using the http package.
// This is a convenience function.
func NewHttpGetter() HttpGetter {
	return &httpGetter{}
}
